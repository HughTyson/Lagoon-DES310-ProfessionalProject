using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.A))
        {
            GM_.Instance.audio.PlaySFX(GM_.Instance.audio.GetSFX("TestFX"),transform, false);
        }

    }
}
