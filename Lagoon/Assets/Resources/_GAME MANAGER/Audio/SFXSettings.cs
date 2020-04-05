using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SFXSettings
{

    public abstract class RootBase
    {
        protected bool isCompleted = false;
        public bool IsCompleted => isCompleted;

        public virtual void Update() { }
    }


    public static class Loop
    {
        public abstract class Base : RootBase
        {
            protected bool isLooping;
            public bool CurrentValue => isLooping;
        }

        public class Constant : Base
        { 
            public Constant(bool isLooping_)
            {
                isLooping = isLooping_;
            }
        }

    }

    public static class Mute
    {
        public abstract class Base : RootBase
        {
            protected bool isMuted;
            public bool CurrentValue => isMuted;
        }

        public class Constant : Base
        {
            public Constant(bool isMuted_)
            {
                isMuted = isMuted_;
            }
        }

    }

    public static class Pitch
    {
        public abstract class Base : RootBase
        {
            protected float pitch;
            public float CurrentValue => pitch;
        }

        public class Constant : Base
        {
            public Constant(float pitch_)
            {
                pitch = pitch_;
            }
        }

    }

    public static class SpatialBlend
    {
        public abstract class Base : RootBase
        {
            protected float spatialBlend;
            public float CurrentValue => spatialBlend;
        }

        public class Constant : Base
        {
            public Constant(float spatialBlend_)
            {
                spatialBlend = spatialBlend_;
            }
        }

    }

    public static class Panning
    {
        public abstract class Base : RootBase
        {
            protected float panning;
            public float CurrentValue => panning;
        }

        public class Constant : Base
        {
            public Constant(float panning_)
            {
                panning = panning_;
            }
        }

    }
    public static class Priority
    {
        public abstract class Base : RootBase
        {
            protected int priority;
            public int CurrentValue => priority;
        }

        public class Constant : Base
        {
            public Constant(int priority_)
            {
                priority = priority_;
            }
        }

    }

    public static class Volume
    {
        public abstract class Base : RootBase
        {
            protected float volume;
            public float CurrentValue => volume;
        }

        public class Constant : Base
        {
            public Constant(float volume_)
            {
                volume = volume_;
            }
        }

    }


}
