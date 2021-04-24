using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    Rigidbody2D body;
    public float speed = 1;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        
        print("Start");
    }

    // Update is called once per frame
    void Update()
    {
        print("Frame");
        body.velocity = new Vector2(speed, body.velocity.y);
        
    }
}
