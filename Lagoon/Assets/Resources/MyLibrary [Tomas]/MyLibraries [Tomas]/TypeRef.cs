using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRef<T> where T : struct
{
    public T value;
    public TypeRef(T val)
    {
        value = val;
    }
    public TypeRef()
    {
    }
}
