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

        TypeRef.Float ValRef;
        float greaterAndEqualTo;
        float lessAndEqualTo;
        public Float(TypeRef.Float valRef_, float greaterAndEqualTo_, float lessAndEqualTo_ )
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
        TypeRef.Int ValRef;
        int greaterAndEqualTo;
        int lessAndEqualTo;
        public Int(TypeRef.Int valRef_, int greaterAndEqualTo_, int lessAndEqualTo_)
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
        TypeRef.Bool ValRef;
        bool whenTrue;
        public Bool(TypeRef.Bool valRef_, bool whenTrue_)
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
