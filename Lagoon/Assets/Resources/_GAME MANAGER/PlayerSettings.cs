using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSettings
{

    public readonly static Vector2 MINMAX_X_SENSITIVITY = new Vector2(0.1f, 10); // Used for the settings sliders
    public readonly static Vector2 MINMAX_Y_SENSITIVITY = new Vector2(0.1f, 10); // Used for the settings sliders

    float xSensitivity = Mathf.Lerp(MINMAX_X_SENSITIVITY.x, MINMAX_X_SENSITIVITY.y, 0.5f); // default sensitivity
    float ySensitivity = Mathf.Lerp(MINMAX_Y_SENSITIVITY.x, MINMAX_Y_SENSITIVITY.y, 0.5f); // default sensitivity
    bool isXInverted = false;
    bool isYInverted = false;


    // Uses methods incase additional things will be changed when values are set
    public float YSensitivity
    {
        get => ySensitivity;
        set => ySensitivity = value;
    }

    public float XSensitivity
    {
        get => xSensitivity;
        set => xSensitivity = value;
    }

    public bool IsYInverted
    {
        get => isYInverted;
        set => isYInverted = value;
    }

    public bool IsXInverted
    {
        get => isXInverted;
        set => isXInverted = value;
    }
    
}
