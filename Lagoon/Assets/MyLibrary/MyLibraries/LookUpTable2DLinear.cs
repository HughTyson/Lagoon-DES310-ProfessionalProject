using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Precalculate data samples and store in a look up table
/// Samples are at the same length appart, but can be different in dimensions
/// floating 'at' index goes in 2 dimensions
/// </summary>
public class LookUpTable2DLinear<T>
{
    private T[] table;


    private readonly int y_size;
    private readonly int x_size;
    private readonly float x_start_at;
    private readonly float y_start_at;
    private readonly float x_increment;
    private readonly float y_increment;

    private int x_filled_slots;
    private int y_filled_slots;



    public LookUpTable2DLinear(int x_size_, float x_start_at_, float x_increment_, int y_size_, float y_start_at_, float y_increment_)
    {
        table = new T[y_size_ * x_size_];


        x_size = x_size_;
        y_size = y_size_;


        x_start_at = x_start_at_;
        y_start_at = y_start_at_;

        x_increment = x_increment_;
        y_increment = y_increment_;
    }
    public LookUpTable2DLinear(float x_start_at_, float x_end_at_, int x_size_, float y_start_at_, float y_end_at_, int y_size_)
    {
        table = new T[y_size_ * x_size_];


        x_size = x_size_;
        y_size = y_size_;

        x_start_at = x_start_at_;
        y_start_at = y_start_at_;

        x_increment = (x_end_at_ - x_start_at) / ((float)(x_size));
        y_increment = (y_end_at_ - y_start_at) / ((float)(y_size));
    }


    /// <summary>
    /// Next sample's At position
    /// </summary>
    public float XNextAt
    {
        get { return x_start_at + (x_increment * (float)x_filled_slots); }
    }
    public float YNextAt
    {
        get { return y_start_at + (y_increment * (float)y_filled_slots); }
    }

    /// <summary>
    /// The current float 'at' index
    /// </summary>
    public float XCurrentAt
    {
        get { return x_start_at + (x_increment * (float)(x_filled_slots - 1)); }
    }
    public float YCurrentAt
    {
        get { return y_start_at + (y_increment * (float)(y_filled_slots - 1)); }
    }

    /// <summary>
    /// Length between sample indexes
    /// </summary>
    public float XAtIncrement
    {
        get { return x_increment; }
    }
    public float YAtIncrement
    {
        get { return y_increment; }
    }

    /// <summary>
    /// Add a sample to the lookup table at constanst increments
    /// </summary>
    public void Add(T value)
    {
        table[(y_filled_slots * x_size) + x_filled_slots] = value;
        x_filled_slots++;

        if (x_filled_slots == x_size)
        {
            x_filled_slots = 0;
            y_filled_slots++;
        }
    }

    /// <summary>
    /// Get the sample at floating index 'at'
    /// </summary>
    public T Get(float x_at, float y_at)
    {
        int x = Mathf.Clamp(Mathf.RoundToInt(((x_at - x_start_at) / x_increment)), 0, x_size - 1);
        int y = Mathf.Clamp(Mathf.RoundToInt(((y_at - y_start_at) / y_increment)), 0, y_size - 1);
        return table[(y * x_size) + x];
    }

    public struct BillinearInterpolateData
    {
        public T data_0_0;
        public T data_1_0;


        public T data_0_1;
        public T data_1_1;

        public float x_t;
        public float y_t;
    }

    public BillinearInterpolateData GetInterpolate(float x_at, float y_at)
    {
        BillinearInterpolateData data = new BillinearInterpolateData();

        float x_t = Mathf.Clamp((x_at - x_start_at) / x_increment, 0.0f, (float)(x_size - 1));
        int x = Mathf.FloorToInt(x_t);
        x_t -= (float)x_t;

        float y_t = Mathf.Clamp(((y_at - y_start_at) / y_increment), 0.0f, (float)(y_size - 1));
        int y = Mathf.FloorToInt(y_t);
        y_t -= (float)y_t;


        bool clip_x = (x == (x_size - 1));
        bool clip_y = (y == (y_size - 1));

        data.data_0_0 = table[(y * x_size) + x];
        data.data_1_0 = (clip_x) ? data.data_0_0 : table[(y * x_size) + (x + 1)];
        data.data_0_1 = (clip_y) ? data.data_0_0 : table[((y + 1) * x_size) + x];

        if (clip_x)
        {
            if (clip_y)
            {
                data.data_1_1 = data.data_0_0;
            }
            else
            {
                data.data_1_1 = data.data_0_1;
            }
        }
        else if (clip_y)
        {
            data.data_1_1 = data.data_1_0;
        }
        else
        {
            data.data_1_1 = table[((y + 1) * x_size) + (x + 1)];
        }



        data.x_t = x_t;
        data.y_t = y_t;

        return data;
    }
}
