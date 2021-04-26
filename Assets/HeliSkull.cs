using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeliSkull : MonoBehaviour
{
    Rigidbody2D body;
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.collider.name == "Player") {
            PlayerController player = collision.collider.GetComponent<PlayerController>();
            Vector2 playerPos = player.body.position;
            Vector2 enemyPos = body.position;
            Vector2 deltaVector = (playerPos - enemyPos);
            player.isJumping = false;
            player.body.AddForce(deltaVector.normalized * 10, ForceMode2D.Impulse);

            triggerEffects();
        }
    }
    void triggerEffects() {
        FXController fXController = GameObject.Find("FX").GetComponent<FXController>();
        SampleController sampleController = GameObject.Find("Audio Controller").GetComponent<SampleController>();
        sampleController.playSound(SoundName.HIT);

        fXController.chromaticHit();
        fXController.bloomHit(10f);

    }
}
