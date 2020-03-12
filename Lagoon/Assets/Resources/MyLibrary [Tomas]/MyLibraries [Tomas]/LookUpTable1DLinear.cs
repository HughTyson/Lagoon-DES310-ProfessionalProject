using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Precalculate data samples and store in a look up table
/// Samples are at the same length appart
/// </summary>
public class LookUpTable1DLinear<T>
{
    private T[] table;
    private int size;
    private int filled_slots;
    private readonly float start_at;
    private readonly float increment;


    public LookUpTable1DLinear(int size_, float start_at_, float increment_)
    {
        table = new T[size_];
        size = size_;
        start_at = start_at_;
        increment = increment_;
    }
    public LookUpTable1DLinear(float start_at_, float end_at_, int size_)
    {
        table = new T[size_];
        size = size_;
        start_at = start_at_;
        increment = (end_at_ - start_at) / ((float)size_);
    }


    /// <summary>
    /// Next sample's At position
    /// </summary>
    public float NextAt
    {
        get { return start_at + (increment * (float)filled_slots); }
    }

    /// <summary>
    /// Starting float 'at' index
    /// </summary>
    public float StartAt
    {
        get { return start_at; }
    }

    /// <summary>
    /// The current float 'at' index
    /// </summary>
    public float CurrentAt
    {
        get { return start_at + (increment * (float)(filled_slots - 1)); }
    }


    /// <summary>
    /// Length between sample indexes
    /// </summary>
    public float AtIncrement
    {
        get { return increment; }
    }
    /// <summary>
    /// Add a sample to the lookup table at constanst increments
    /// </summary>
    public void Add(T value)
    {
        table[filled_slots] = value;
        filled_slots++;
    }

    /// <summary>
    /// Get the sample at floating index 'at'
    /// </summary>
    public T Get(float at)
    {
        return table[Mathf.Clamp(Mathf.RoundToInt(((at - start_at) / increment)), 0, size - 1)];
    }


    public struct InterpolateData
    {
        public T start;
        public T end;
        public float t;
    }

    /// <summary>
    /// Get the data required for interpolating between 2 samples 
    /// </summary>
    public InterpolateData GetInterpolated(float at)
    {
        float t = Mathf.Clamp(((at - start_at) / increment), 0.0f, (float)(size - 1));
        int i = Mathf.FloorToInt(t);
        t -= (float)i;

        InterpolateData data = new InterpolateData();
        data.start = table[i];
        data.end = (i == size - 1) ? data.start : table[i + 1];
        data.t = t;

        return data;
    }
}
