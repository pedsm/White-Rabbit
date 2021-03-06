using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleDevil : MonoBehaviour
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
        if (nextChangeDir > Time.time)
        {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.name == "Player")
        {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            // Vector2 playerPos = player.body.position;
            // Vector2 enemyPos = body.position;
            // Vector2 deltaVector = (playerPos - enemyPos);
            player.isJumping = false;
            // player.body.AddForce(deltaVector.normalized * 10, ForceMode2D.Impulse);

            triggerEffects();
        }
    }
    void triggerEffects()
    {
        FXController fXController = GameObject.Find("FX").GetComponent<FXController>();
        SampleController sampleController = GameObject.Find("Audio Controller").GetComponent<SampleController>();
        sampleController.playSound(SoundName.HIT);

        // fXController.chromaticHit();
        fXController.bloomHit(10f);

    }
}
