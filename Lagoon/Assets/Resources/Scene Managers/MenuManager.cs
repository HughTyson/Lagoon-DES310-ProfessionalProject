using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    AudioSFX waves_noise;

    // Start is called before the first frame update
    void Start()
    {
        
        if(GM_.Instance.audio.GetFirstSFXInstanceUsingAppliedID("WAVE_NOISES") != null)
        {




        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
