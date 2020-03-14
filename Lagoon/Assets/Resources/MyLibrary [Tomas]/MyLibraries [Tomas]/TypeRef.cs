using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRef<T>
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
