using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioSettings
{

    public abstract class RootBase
    {
        protected bool isCompleted = false;
        public bool IsCompleted => isCompleted;

        public virtual void Update() { }
    }


    public static class AnyFloatSetting
    {
        public abstract class FloatBase : RootBase
        {
            protected float value;
            public float Value => value;
        }

        public class Constant : FloatBase
        {
            public Constant(float value_)
            {
                value = value_;
            }
        }

        public class Velocity : FloatBase
        {

            TypeRef<float> velocity;
            float min;
            float max;


            public Velocity(TypeRef<float> velocityRef, float startingValue, float minValue, float maxValue)
            {
                velocity = velocityRef;
                min = minValue;
                max = maxValue;
                value = startingValue;
            }

            public override void Update()
            {
                value += velocity.value * Time.unscaledDeltaTime;
                value = Mathf.Clamp(value, min, max);
            }
        }

        public class SmoothStep : FloatBase
        {

            TypeRef<float> desiredValue;
            float speed;

            public SmoothStep(TypeRef<float> desiredValue_, float startingValue_, float speed_ = 1)
            {
                desiredValue = desiredValue_;
                value = startingValue_;
                speed = speed_;
            }

            public override void Update()
            {
                value = Mathf.Lerp(value, desiredValue.value, Time.deltaTime * speed);
            }
        }
    }
    public static class AnyBoolSetting
    {
        public abstract class BoolBase : RootBase
        {
            protected bool value;
            public bool Value => value;
        }

        public class Constant : BoolBase
        {
            public Constant(bool value_)
            {
                value = value_;
            }
        }
    }


    public static class AnyIntSetting
    {
        public abstract class IntBase : RootBase
        {
            protected int value;
            public int Value => value;
        }

        public class Constant : IntBase
        {
            public Constant(int value_)
            {
                value = value_;
            }
        }

    }




    public static class LoopOnly
    {


    }

    public static class MuteOnly
    {


    }

    public static class PitchOnly
    {
 


        

    }

    public static class SpatialBlendOnly
    {


    }

    public static class PanningOnly
    {


    }
    public static class Priority
    {

    }

    public static class VolumeOnly
    {

    }



    public static class MusicOnly
    {
        public static class Volume
        {
            public class CrossFade
            {


            }
            public class FadeOut
            {

            }
            public class FadeIn
            {

            }

        }




    }


}
