using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueWithDefault<T> where T : struct
{
    T default_value;
    T current_value;

    public T Default { get { return default_value; } }
    public T Current { get { return current_value; } }
    public void SetDefault(T default_value_)
    {
        default_value = default_value_;
        current_value = default_value;
    }

    public void Reset()
    {
        current_value = default_value;
    }
    public void SetCurrent(T current_value_)
    {
     
        current_value = current_value_;
    }
}
