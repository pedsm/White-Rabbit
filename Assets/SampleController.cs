using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    SOUNDTRACK,
    JUMP,
    LAND,
    DAMAGED,
    HIT
}

public class SampleController : MonoBehaviour {

    private AudioSource[] sounds;

    // sound declarations
    private AudioSource soundtrack;
    private AudioSource jump;
    private AudioSource land;
    private AudioSource damaged;
    private AudioSource hit;

    void Start() {

        sounds = GetComponents<AudioSource>();

        soundtrack = sounds[(int)SoundName.SOUNDTRACK];
        jump = sounds[(int)SoundName.JUMP];
        land = sounds[(int)SoundName.LAND];
        damaged = sounds[(int)SoundName.DAMAGED];
        hit = sounds[(int)SoundName.HIT];

    }

    public void playSound(SoundName soundName) {
        sounds[(int)soundName].Play();
    }

    public void playSound(SoundName soundName, float volume) {
        if(volume > 1) {
            volume = 1f;
        }

        sounds[(int)soundName].volume = volume;
        sounds[(int)soundName].Play();
    }
}
