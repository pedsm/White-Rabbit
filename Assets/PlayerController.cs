using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    public Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    public float speed = 2;
    public float jumpForce = 5;

    private float jumpTimeCounter = 0.35f;
    public float jumpTime;
    public bool isJumping;
    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius = 0.06f;
    public LayerMask groundLayer;
    public Animator animator;

    private float maxYVel = 0;
    private float fallDmgThreshold = 10f;
    public SampleController sampleController;

    private int? lastContactCount = null;

    public int hp = 100;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isAlive()) {
            Move();
            Jump();
            CheckIfGrounded();
            // FallDamageChecker();
        } else {
            body.velocity = new Vector2(0,0);
            animator.SetBool("Alive", isAlive());
            animator.SetBool("isJumping", false);
        }
    }

    void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        spriteRenderer.flipX = x < 0;
        float xDelta = x * speed;
        animator.SetFloat("Speed", Mathf.Abs(xDelta));
        Vector2 targetVelocity = new Vector2(xDelta, body.velocity.y);
        Vector2 velocity = body.velocity;
        Vector2 velocityChange = targetVelocity - velocity;
        body.AddForce(velocityChange, ForceMode2D.Impulse);
    }


    void Jump()
    {
        animator.SetBool("isJumping", !isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            maxYVel = 0;
            Vector2 targetVelocity = new Vector2(body.velocity.x, jumpForce);
            Vector2 velocity = body.velocity;
            Vector2 velocityChange = targetVelocity - velocity;
            body.AddForce(velocityChange, ForceMode2D.Impulse);
            sampleController.playSound(SoundName.JUMP);
        }

        if(Input.GetKey(KeyCode.Space) && isJumping == true) {
            if (jumpTimeCounter > 0) {
                Vector2 targetVelocity = new Vector2(body.velocity.x, jumpForce);
                Vector2 velocity = body.velocity;
                Vector2 velocityChange = targetVelocity - velocity;
                body.AddForce(velocityChange, ForceMode2D.Impulse);
                jumpTimeCounter -= Time.deltaTime;
            } else {
                isJumping = false;
            }
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            isJumping = false;
        }

    }

    void FallDamageChecker() {
        if(!isGrounded) {
            if(maxYVel > body.velocity.y) {
                maxYVel = body.velocity.y;
            }
        }
        if(isGrounded && body.velocity.y == 0) {
            float absoluteVel = Mathf.Abs(maxYVel);
            // playing fall sound with differing volume based on impact
            if (absoluteVel > 2) {
                sampleController.playSound(SoundName.LAND, absoluteVel/13);
            }
            // fall damage calculation
            if(absoluteVel > fallDmgThreshold) {
                print("Take damage" + maxYVel.ToString());
                takeDamage(Mathf.Pow(absoluteVel - fallDmgThreshold, 2));
                print(absoluteVel/25);
                sampleController.playSound(SoundName.DAMAGED, absoluteVel/25);
            }
            maxYVel = 0;
        }
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.name == "Floors_Walls") {
            collide(collision);
        }
    }
    void collide(Collision2D collision) {
        float absoluteVel = collision.relativeVelocity.magnitude;
        if (absoluteVel > 2) {
            sampleController.playSound(SoundName.LAND, absoluteVel/13);
        }
        if(absoluteVel > fallDmgThreshold) {
            print("Take damage" + maxYVel.ToString());
            takeDamage(Mathf.Pow(absoluteVel - fallDmgThreshold, 2));
            sampleController.playSound(SoundName.DAMAGED, absoluteVel/15);
        }
        var contacts = new List<ContactPoint2D>();
        collision.GetContacts(contacts);
        foreach(var contact in contacts) {
            var magnitude = contact.point.magnitude / 100;
            Debug.DrawRay(
                new Vector3(contact.point.x, contact.point.y, 0f),
                new Vector3(contact.normal.x * magnitude, contact.normal.y * magnitude, 0f),
                Color.red,
                1f
            );
        }
        lastContactCount = collision.contactCount;
    }

    void OnCollisionStay2D(Collision2D collision) {
        if(collision.collider.name == "Floors_Walls") {
            var normal = collision.GetContact(0).normal;
            if(collision.contactCount == lastContactCount) {
                return;
            }
            collide(collision);
        }
        
    }
    void OnCollisionExit2D(Collision2D other) {
        lastContactCount = null;
    }

    void takeDamage(float dmgValue) {
        hp = hp - Mathf.RoundToInt(dmgValue);
    }

    void CheckIfGrounded() {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        isGrounded = collider != null;
    }

    bool isAlive() {
        return hp > 0;
    }

}