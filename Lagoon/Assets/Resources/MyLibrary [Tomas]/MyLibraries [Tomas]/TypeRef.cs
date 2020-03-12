using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeRef
{
    public class Float
    {
        public float value;
        public Float(float val)
        {
            value = val;
        }
        public Float() { }

    }
    public class Int
    {
        public int value;
        public Int(int val)
        {
            value = val;
        }
        public Int() { }

    }
    public class Bool
    {
        public bool value;
        public Bool(bool val)
        {
            value = val;
        }
        public Bool() { }
    }
}
