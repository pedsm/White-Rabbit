using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
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

        soundtrack = sounds[(int)SoundName.SOUNDTRACK];
        jump = sounds[(int)SoundName.JUMP];

    }

    public void playSound(SoundName soundName) {
        sounds[(int)soundName].Play();

    }

    // public void playJump() {
    //     jump.Play();
    // }
}
