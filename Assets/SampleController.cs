using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum soundName
{
    SOUNDTRACK,
    JUMP
}

public class SampleController : MonoBehaviour {

    private AudioSource[] sounds;

    // sound declarations
    private AudioSource soundtrack;
    private AudioSource jump;

    void Start() {

        sounds = GetComponents<AudioSource>();

        soundtrack = sounds[(int)soundName.SOUNDTRACK];
        jump = sounds[(int)soundName.JUMP];

    }

    public void playJump() {
        jump.Play();
    }
}
