using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMover : MonoBehaviour
{
    public Vector2 vel;
    Rigidbody2D body;
    float nextChangeDir = 0f;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        setSpeed();
    }

    void setSpeed()
    {
        if(nextChangeDir > Time.time) {
            return;
        }
        vel = new Vector2(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        );
        nextChangeDir = Time.time + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        setSpeed();
        body.velocity = vel;
    }


}
