using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection.Emit;
using System;
using System.Reflection;

[CreateAssetMenu(fileName = "TweenCurveLibrary")]



public class TweenCurveLibrary : ScriptableObject
{

    [Serializable]
    public class Curve
    {
        public string NAME;
        public AnimationCurve curve;
    }

    [Space]
    [Header("Last key of curve must be at (1,1)")]
    [Space]
    [Header("First key of curve must be at (0,0)")]
    [Space]
    [Header("IMPORTANT")]
    [SerializeField]
    List<Curve> curves;

    Dictionary<object, AnimationCurve> curve_dictionary = new Dictionary<object, AnimationCurve>();
    
    public void Init()
    {
        for (int i = 0; i < curves.Count; i++)
        {
            curve_dictionary.Add(curves[i].NAME, curves[i].curve);
        }
    }
    public AnimationCurve GetCurve(object key)
    {
        return curve_dictionary[key];
    }
}
