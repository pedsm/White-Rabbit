using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{
    Rigidbody2D body;
    SpriteRenderer spriteRenderer;
    public float speed = 2;
    public float jumpForce = 5;
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Jump();
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
        // Change this back when is grounded is introduced
        // animator.SetBool("isJumping", body.velocity.y != 0);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            body.velocity = Vector2.up * jumpForce;
        }

    }
}
