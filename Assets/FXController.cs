using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class FXController : MonoBehaviour
{
    Volume volume;
    ChromaticAberration chromaticAberration;
    Bloom bloom;
    LensDistortion lensDistortion;

    float chromaticAberrationIntesity = 0f;
    float bloomIntesity = 0f;
    float lensDistortionIntesity = 0f;
    bool tripping = false;
    public float tripStart = 0;

    public float decayValue = 0.03f;
    
    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<Volume>(); 
        volume.profile.TryGet<ChromaticAberration>(out chromaticAberration);
        volume.profile.TryGet<Bloom>(out bloom);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    // Update is called once per frame
    void Update()
    {
        chromaticAberration.intensity.Override(chromaticAberrationIntesity);
        bloom.intensity.Override(bloomIntesity);
        if(tripping) {
            float tripPercent = (Time.time - tripStart)/5;
            if(tripPercent > 1) {
                tripPercent = 2 - tripPercent;
            }
            lensDistortion.intensity.Override(Mathf.Lerp(0, 1f, tripPercent));
            lensDistortion.scale.Override(Mathf.Lerp(1f, 0.01f, tripPercent));
            if(Time.time - tripStart > 10) {
                tripping = false;
                lensDistortion.intensity.Override(0);
                lensDistortion.scale.Override(1f);
            }
        }
        decay();
    }

    void decay() {
        chromaticAberrationIntesity = decayWithMin(chromaticAberrationIntesity);
        bloomIntesity = decayWithMin(bloomIntesity);
        // lensDistortionIntesity = decayWithMin(lensDistortionIntesity);
        if(tripping) {
            lensDistortionIntesity = Mathf.Sin(Time.time);
        }
    }

    float decayWithMin(float value) {
        value = value - (value * decayValue);
        if(value <= 0f) {
            value = 0f;
        }
        return value;
    }

    public void chromaticHit() {
        chromaticAberrationIntesity = 1f;
    }

    public void bloomHit() {
        bloomHit(1f);
    }
    public void bloomHit(float intensity) {
        bloomIntesity = intensity;
    }
    public void lensHit() {
        lensDistortionIntesity = 1f;
    }

    public void trip() {
        Debug.Log("Start trip");
        lensDistortionIntesity = 1f;
        lensDistortion.scale.Override(0.54f);
        tripping = true;
        tripStart = Time.time;
    }
}
