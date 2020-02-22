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


public static class RandomnessExtension
{

    public static float RandomRangeWithNormalDistribution(float min_, float max_, float edgeFactor = 1.6f)
    {
        float inner_value = (Random.value - 0.5f) * edgeFactor;
        return Mathf.Lerp(min_,max_,(inner_value* inner_value* inner_value) + 0.5f);
    }

    public static float GetNormalDistributionValue(float at_t, float edgeFactor = 1.6f )
    {
        at_t = Mathf.Clamp01(at_t);
        float inner_value = (at_t - 0.5f) * edgeFactor;
        return ((inner_value * inner_value * inner_value) + 0.5f);
    }
}

public static class ListExtension
{ 
    public static void RemoveNullReferences<T>(this List<T> list)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i] == null)
                list.Remove(list[i]);
        }
    }

}
