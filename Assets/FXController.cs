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

    public float decayValue = 0.001f;
    
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
        lensDistortion.intensity.Override(lensDistortionIntesity);
        decay();
    }

    void decay() {
        chromaticAberrationIntesity = decayWithMin(chromaticAberrationIntesity);
        bloomIntesity = decayWithMin(bloomIntesity);
        lensDistortionIntesity = decayWithMin(lensDistortionIntesity);
    }

    float decayWithMin(float value) {
        value = value - decayValue;
        if(value <= 0f) {
            value = 0f;
        }
        return value;
    }

    public void chromaticHit() {
        chromaticAberrationIntesity = 1f;
    }
    public void bloomHit() {
        bloomIntesity = 1f;
    }
    public void lensHit() {
        lensDistortionIntesity = 1f;
    }
}
