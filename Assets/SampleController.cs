using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundName
{
    SOUNDTRACK,
    JUMP,
    LAND,
    DAMAGED,
    HIT,
    ARPEG
}

public class SampleController : MonoBehaviour {

    public AudioMixer arpegio;

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

        StartCoroutine(StartFade(arpegio, "volume", 20f, 1f));

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

     public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume)
    {
        float currentTime = 0;
        float currentVol;
        audioMixer.GetFloat(exposedParam, out currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }
        yield break;
    }
}
