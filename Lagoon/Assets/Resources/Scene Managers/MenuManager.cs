using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    AudioSFX waves_noise;
    AudioManager.SFXInstanceInterface waves_noise_handler;
    //[SerializeField] RadioSignal signal;
    // Start is called before the first frame update
    void Start()
    {
        
        if(GM_.Instance.audio.GetFirstSFXInstanceUsingAppliedID("OCEAN_NOISE") != null)
        {
            waves_noise_handler = GM_.Instance.audio.GetFirstSFXInstanceUsingAppliedID("OCEAN_NOISE");
        }
        else
        {
            waves_noise = GM_.Instance.audio.GetSFX("OceanNoise");
            waves_noise_handler = GM_.Instance.audio.PlaySFX(waves_noise, null, appliedID: "OCEAN_NOISE");
        }

    }

    // Update is called once per frame
    void Update()
    {
        GM_.Instance.day_night_cycle.SetTime(0.0f);

        //signal.enabled = false;
    }
}
