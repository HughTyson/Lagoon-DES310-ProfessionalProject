using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testScript : MonoBehaviour
{
    // Update is called once per frame


    private void Start()
    {

    }
    void Update()
    {

        if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.LB))
        {

        }
        else if (GM_.Instance.input.GetButtonDown(InputManager.BUTTON.RB))
        {

        }
    }
}
