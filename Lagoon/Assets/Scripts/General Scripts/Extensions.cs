using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Extension
{

    // https://answers.unity.com/questions/661383/whats-the-most-efficient-way-to-rotate-a-vector2-o.html
    public static Vector2 Rotate(this Vector2 v, float degrees)
    {
        float sin = Mathf.Sin(degrees * Mathf.Deg2Rad);
        float cos = Mathf.Cos(degrees * Mathf.Deg2Rad);

        float tx = v.x;
        float ty = v.y;
        v.x = (cos * tx) - (sin * ty);
        v.y = (sin * tx) + (cos * ty);
        return v;
    }
    public static Vector2 Slerp(Vector2 from, Vector2 to, float t)
    {
        float angle_between = Vector2.SignedAngle(from, to) * Mathf.Clamp01(t);
        return Rotate(from, angle_between);
    }
}
