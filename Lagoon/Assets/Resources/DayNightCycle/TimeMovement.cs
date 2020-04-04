using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeMovement : MonoBehaviour
{

    public enum Solar
    {
        DAY,
        NIGHT
    }

    Solar solar;
    
    float time_multiplyer = 1.0f;

    public void SetTime(float time_multi)
    {
        time_multiplyer = time_multi;
    }

    public float GetTime()
    {
        return time_multiplyer;
    }

    public void SetSolar(Solar s)
    {
        solar = s;
    }

    public Solar GetSolar()
    {
        return solar;
    }

}
