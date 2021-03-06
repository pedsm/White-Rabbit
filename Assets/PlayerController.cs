using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    private bool flipped = false;

    private int? lastContactCount = null;

    public int hp = 175;

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
            Restart();
        }
    }
    void Restart() {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) {
          SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void Move()
    {
        float x = flipped ? Input.GetAxisRaw("Horizontal") * -1 : Input.GetAxisRaw("Horizontal") ;
        spriteRenderer.flipX = flipped ? x > 0 : x < 0;
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
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton1)) && isGrounded)
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

        if((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.JoystickButton1)) && isJumping == true) {
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

        if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.JoystickButton1)) {
            isJumping = false;
        }

    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.name == "Floors_Walls") {
            collide(collision);
        }
    }
    void collide(Collision2D collision) {
        if(!isAlive()) {
            return;
        }
        float absoluteVel = collision.relativeVelocity.magnitude;
        if (absoluteVel > 5) {
            sampleController.playSound(SoundName.LAND, absoluteVel/13);
        }
        if(absoluteVel > fallDmgThreshold) {
            takeDamage(Mathf.Pow(absoluteVel - fallDmgThreshold, 2));
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

    public void takeDamage(float dmgValue) {
        if(!isAlive()) {
            return;
        }
        if(dmgValue > 5) {
            hp = hp - Mathf.RoundToInt(dmgValue);
            print("Dmg:" + dmgValue.ToString());
            float dmgVolume = Mathf.Lerp(0.3f, 1, dmgValue/100);
            sampleController.playSound(SoundName.DAMAGED, dmgVolume);
        }
    }

    void CheckIfGrounded() {
        Collider2D collider = Physics2D.OverlapCircle(isGroundedChecker.position, checkGroundRadius, groundLayer);
        isGrounded = collider != null;
    }

    bool isAlive() {
        return hp > 0;
    }

    void flip() {
        if(flipped) {
            return;
        }
        flipped = true;
        body.gravityScale = -2;
        GetComponent<Transform>().rotation = Quaternion.Euler(0,0,180f);
        GameObject.Find("Camera").GetComponent<Camera>().GetComponent<Transform>().rotation = Quaternion.Euler(0,0,180f);
        GameObject.Find("Timer").GetComponent<Timer>().flip();
        jumpForce = jumpForce * -1;
    }

    void trip() {
        GameObject.Find("FX").GetComponent<FXController>().trip();
        fallDmgThreshold = 100f;
    }
    private void OnTriggerEnter2D(Collider2D trigger) {
        int stage = int.Parse(trigger.name);
        if(stage == 4) {
            if(!flipped) {
                trip();
            }
        }
        if(stage == 5) {
            flip();
        }
        if(stage == 6) {
            GameObject.Find("Timer").GetComponent<Timer>().finishGame();
        }
        sampleController.setCurrentStage(stage);
    }


}