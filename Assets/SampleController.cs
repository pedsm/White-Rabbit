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
    ARPEG,
    PADS,
    KICK,
    BLACKHOLE,
    OUTRO
}

public class SampleController : MonoBehaviour {

    public AudioMixer mixer;

    private AudioSource[] sounds;

    // sound declarations
    private AudioSource jump;
    private AudioSource land;
    private AudioSource damaged;
    private AudioSource hit;
    private AudioSource blackhole;
    private AudioSource outro;

    private int currentStage = 0;

    void Start() {
        sounds = GetComponents<AudioSource>();
        //initialise mixer levels
        mixer.SetFloat("arpeg_volume", -80f);
        mixer.SetFloat("pad_volume", -80f);
        mixer.SetFloat("kick_volume", -80f);
        mixer.SetFloat("soundtrack_volume", 0f);
        mixer.SetFloat("outro_volume", -80f);
    }

    public void playSound(SoundName soundName) {
        sounds[(int)soundName].Play();
    }

    public void playDelayedSound(SoundName soundName, float delay) {
        sounds[(int)soundName].PlayDelayed(delay);
    }

    public void playSound(SoundName soundName, float volume) {
        if(volume > 1) {
            volume = 1f;
        }
        sounds[(int)soundName].volume = volume;
        sounds[(int)soundName].Play();
    }

     public static IEnumerator StartFade(AudioMixer audioMixer, string exposedParam, float duration, float targetVolume, float delay)
    {
        float currentTime = 0 - delay;
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


    public void setCurrentStage(int newStage) {
        if(currentStage < newStage) {
           Debug.Log("Entering stage: "+ newStage);
           currentStage = newStage;

           if (currentStage == 1) {
                StartCoroutine(StartFade(mixer, "arpeg_volume", 20f, 0f, 0f));
           }
           if (currentStage == 2) {
                mixer.SetFloat("arpeg_volume", 1f);
                StartCoroutine(StartFade(mixer, "pad_volume", 20f, 0f, 0f));
           }
           if (currentStage == 3) {
                mixer.SetFloat("arpeg_volume", 0f);
                mixer.SetFloat("pad_volume", 0f);
                mixer.SetFloat("kick_volume", 0f);
           }
           if (currentStage == 4) {
               //uncoment these if you want to test
                mixer.SetFloat("arpeg_volume", 0f);
                mixer.SetFloat("pad_volume", 0f);
                mixer.SetFloat("kick_volume", 0f);
                playSound(SoundName.BLACKHOLE);
                StartCoroutine(StartFade(mixer, "soundtrack_volume", 8f, 0f, 0f));
                // delay and bring in outro
                // playDelayedSound()
                playDelayedSound(SoundName.OUTRO, 8f);
                StartCoroutine(StartFade(mixer, "outro_volume", 6f, 1f, 8f));
           }
        }
    }
}
