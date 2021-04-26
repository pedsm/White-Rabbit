using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    public Vector2 vel = new Vector2(0, 2f);
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = vel;
    }

    void OnCollisionEnter2D(Collision2D other) {
        vel = new Vector2(0, -vel.y);

    }
}
