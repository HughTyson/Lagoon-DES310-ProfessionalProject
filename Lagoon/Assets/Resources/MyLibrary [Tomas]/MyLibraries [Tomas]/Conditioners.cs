using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Conditioners
{
    public abstract class BaseC
    {
        public abstract bool IsConditionMet();
    }


    public class Float : BaseC
    {

        TypeRef<float> ValRef;
        float greaterAndEqualTo;
        float lessAndEqualTo;
        public Float(TypeRef<float> valRef_, float greaterAndEqualTo_, float lessAndEqualTo_ )
        {
            ValRef = valRef_;
            lessAndEqualTo = lessAndEqualTo_;
            greaterAndEqualTo = greaterAndEqualTo_;
        }
        public override bool IsConditionMet() 
        {
            return (ValRef.value >= greaterAndEqualTo && ValRef.value <= lessAndEqualTo);
        }
    }
    public class Int : BaseC
    {
        TypeRef<int> ValRef;
        int greaterAndEqualTo;
        int lessAndEqualTo;
        public Int(TypeRef<int> valRef_, int greaterAndEqualTo_, int lessAndEqualTo_)
        {
            ValRef = valRef_;
            lessAndEqualTo = lessAndEqualTo_;
            greaterAndEqualTo = greaterAndEqualTo_;
        }
        public override bool IsConditionMet()
        {
            return (ValRef.value >= greaterAndEqualTo && ValRef.value <= lessAndEqualTo);
        }
    }
    public class Bool : BaseC
    {
        TypeRef<bool> ValRef;
        bool whenTrue;
        public Bool(TypeRef<bool> valRef_, bool whenTrue_)
        {
            ValRef = valRef_;
            whenTrue = whenTrue_;
        }
        public override bool IsConditionMet()
        {
            return (ValRef.value == whenTrue);
        }
    }




}
