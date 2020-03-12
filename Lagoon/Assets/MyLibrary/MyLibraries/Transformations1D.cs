using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Use these in lerp's t to get different distribution curves
/// </summary>
static public class Transformations1D
{
    public static float EaseIn(float t)
    {
        return t * t;
    }
    public static float EaseOut(float t)
    {
        return 1.0f - ((1.0f - t) * (1.0f - t));
    }

    public static float EaseInOut(float t)
    {
        return (3.0f * t * t - 2.0f * t * t * t);
    }
    public static float NotEaseInOut(float t)
    {
        float inner = (t - 0.5f) * 1.6f;
        return (inner * inner * inner) + 0.5f;
    }


}
