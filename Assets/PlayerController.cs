using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    public float speed = 2;
    public float jumpForce = 5;

    private float jumpTimeCounter = 0.35f;
    public float jumpTime;
    private bool isJumping;
    bool isGrounded = false;
    public Transform isGroundedChecker;
    public float checkGroundRadius = 0.06f;
    public LayerMask groundLayer;
    public Animator animator;

    private float maxYVel = 0;
    private float fallDmgThreshold = 10f;
    public SampleController sampleController;

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
            FallDamageChecker();
        } else {
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
        body.velocity = new Vector2(xDelta, body.velocity.y);
    }


    void Jump()
    {
        animator.SetBool("isJumping", !isGrounded);
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            isJumping = true;
            jumpTimeCounter = jumpTime;
            maxYVel = 0;
            body.velocity = new Vector2(body.velocity.x, jumpForce);
            sampleController.playJump();
        }

        if(Input.GetKey(KeyCode.Space) && isJumping == true) {
            if (jumpTimeCounter > 0) {
                body.velocity = new Vector2(body.velocity.x, jumpForce);
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
            if(absoluteVel > fallDmgThreshold) {
                print("Take damage" + maxYVel.ToString());
                takeDamage(Mathf.Pow(absoluteVel - fallDmgThreshold, 2));
                maxYVel = 0;
            }
        }
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
