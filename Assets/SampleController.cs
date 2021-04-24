using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundName
{
    SOUNDTRACK,
    JUMP,
    LAND
}

public class SampleController : MonoBehaviour {

    private AudioSource[] sounds;

    // sound declarations
    private AudioSource soundtrack;
    private AudioSource jump;
    private AudioSource land;

    void Start() {

        sounds = GetComponents<AudioSource>();

        soundtrack = sounds[(int)SoundName.SOUNDTRACK];
        jump = sounds[(int)SoundName.JUMP];
        land = sounds[(int)SoundName.LAND];

    }

    public void playSound(SoundName soundName) {
        sounds[(int)soundName].Play();

    }

    public void playSound(SoundName soundName, float volume) {
        sounds[(int)soundName].volume = volume;
        sounds[(int)soundName].Play();
    }



}
