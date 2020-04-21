using System.Collections.Generic;
using UnityEngine;

public class TweenAnimator
{
    public class Animation
    {
        public class PlayArgs
        {
            public readonly System.Action animationUpdatedDelegate;
            public readonly System.Action animationCompleteDelegate;
            public readonly TweenManager.PATH path;
            public readonly TweenManager.DIRECTION startingDirection;
            public readonly TweenManager.TIME_FORMAT TimeFormat;
            public readonly float speed;
            public readonly object instanceID;
            public readonly float startingPercentageOfCompletion;
            public PlayArgs(System.Action animationUpdatedDelegate_ = null, System.Action animationCompleteDelegate_ = null, TweenManager.PATH path_ = TweenManager.PATH.NORMAL, TweenManager.DIRECTION startingDirection_ = TweenManager.DIRECTION.START_TO_END, TweenManager.TIME_FORMAT TimeFormat_ = TweenManager.TIME_FORMAT.DELTA, float speed_ = 1, object instanceID_ = null, float startingPercentageOfCompletion_ = 0.0f)
            {
                animationUpdatedDelegate = animationUpdatedDelegate_;
                animationCompleteDelegate = animationCompleteDelegate_;
                path = path_;
                startingDirection = startingDirection_;
                TimeFormat = TimeFormat_;
                speed = speed_;
                instanceID = instanceID_;
                startingPercentageOfCompletion = startingPercentageOfCompletion_;
            }
        }


        TweenManager.TweenInstanceInterface tweenInterface;
        public TweenManager.TweenInstanceInterface TweenInterface => tweenInterface;
        public bool IsPlaying { 
            get 
            {
                if (tweenInterface == null)
                {
                    return false;
                }
                return tweenInterface.Exists;
            } 
        }

        Base.Property[] changingProperties;

        TypeRef<float>[] tweenOutputs;

        TweenManager.TweenPathBundle tweenBundle;

        System.Action animationUpdatedDelegate;
        System.Action animationCompleteDelegate;

        public Animation(TweenManager.TweenPathBundle tweenBundle_, params Animatable[] animatables_)
        {
            tweenBundle = tweenBundle_;

            List<Base.Property> changingPropertiesList = new List<Base.Property>();
            for (int i = 0; i < animatables_.Length; i++)
                changingPropertiesList.AddRange(animatables_[i].ChangingProperties);

            changingProperties = changingPropertiesList.ToArray();

            tweenOutputs = new TypeRef<float>[tweenBundle_.NumberOfTweenedValues];
            for (int i = 0; i < tweenOutputs.Length; i++)
                tweenOutputs[i] = new TypeRef<float>();

            for (int i = 0; i < changingProperties.Length; i++)
                changingProperties[i].SetTweenLinks(tweenOutputs);
        }

        public void PlayAnimation(System.Action animationUpdatedDelegate_ = null, System.Action animationCompleteDelegate_ = null, TweenManager.PATH path_ = TweenManager.PATH.NORMAL, TweenManager.DIRECTION startingDirection_ = TweenManager.DIRECTION.START_TO_END, TweenManager.TIME_FORMAT TimeFormat_ = TweenManager.TIME_FORMAT.DELTA, float speed_ = 1, object instanceID = null, float startingPercentageOfCompletion = 0.0f) // allow the same things as a tween instance, but make deleagate of its own to make sure it happens last
        {
            if (IsPlaying)
            {
                tweenInterface.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE);
            }

            for (int i = 0; i < changingProperties.Length; i++)
            {
                changingProperties[i].Start();
            }
            animationUpdatedDelegate = animationUpdatedDelegate_;
            animationCompleteDelegate = animationCompleteDelegate_;
            tweenInterface = GM_.Instance.tween_manager.StartTweenInstance(tweenBundle, tweenOutputs, Update, Completed, path_: path_, startingDirection_: startingDirection_, TimeFormat_: TimeFormat_, speed_: speed_, instanceID: instanceID, startingPercentageOfCompletion: startingPercentageOfCompletion);// include other parameters
        }
        public void PlayAnimation(PlayArgs args, System.Action unique_animationUpdatedDelegate_ = null, System.Action unique_animationCompleteDelegate_ = null)
        {
            if (IsPlaying)
            {
                tweenInterface.StopTween(TweenManager.STOP_COMMAND.IMMEDIATE);
            }

            for (int i = 0; i < changingProperties.Length; i++)
            {
                changingProperties[i].Start();
            }
            animationUpdatedDelegate = args.animationUpdatedDelegate + unique_animationUpdatedDelegate_;
            animationCompleteDelegate = args.animationCompleteDelegate + unique_animationCompleteDelegate_;
            tweenInterface = GM_.Instance.tween_manager.StartTweenInstance(tweenBundle, tweenOutputs,tweenUpdatedDelegate_: Update,tweenCompleteDelegate_: Completed, path_: args.path, startingDirection_: args.startingDirection, TimeFormat_: args.TimeFormat, speed_: args.speed, instanceID: args.instanceID, startingPercentageOfCompletion: args.startingPercentageOfCompletion);// include other parameters
        }


        public void StopAnimation(TweenManager.STOP_COMMAND stopCommand)
        {
            if (IsPlaying)
            {
                tweenInterface.StopTween(stopCommand);
            }
        }

        void Update()
        {
            for (int i = 0; i < changingProperties.Length; i++)
            {
                changingProperties[i].UpdateProperty();
            }
            animationUpdatedDelegate?.Invoke();
        }

        void Completed()
        {
            animationCompleteDelegate?.Invoke();
        }
    }



    public abstract class Animatable
    {
        protected List<Base.Property> changingProperties = new List<Base.Property>();
        public List<Base.Property> ChangingProperties => changingProperties;
    }

    public enum MOD_TYPE
    { 
    ABSOLUTE,
    OFFSET
    }

    public class Base
    {
        public abstract class Property
        {
            protected MOD_TYPE modification_type;
            protected object reference;

            protected TypeRef<float>[] tweenLinks;
            public void SetTweenLinks(TypeRef<float>[] tweenLinks_)
            {
                tweenLinks = tweenLinks_;
            }
            public void Init(object reference_)
            {
                reference = reference_;
            }
            public abstract void Start();
            public abstract void UpdateProperty();
        }
        public abstract class Vector3Property : Property
        {
            Vector3 default_value;
            Vector3 current_value;

            bool modifyX;
            int X_tweenOutputIndex;
            bool modifyY;
            int Y_tweenOutputIndex;
            bool modifyZ;
            int Z_tweenOutputIndex;
            protected abstract Vector3 ReferenceValue { get; set; }

            public sealed override void Start()
            {
                default_value = ReferenceValue;
            }
            public sealed override void UpdateProperty()
            {
                Vector3 untouchedPosition = ReferenceValue;

                switch (modification_type)
                {
                    case MOD_TYPE.ABSOLUTE:
                        {
                            if (modifyX)
                            {
                                current_value.x = tweenLinks[X_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.x = untouchedPosition.x;
                            }
                            if (modifyY)
                            {
                                current_value.y = tweenLinks[Y_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.y = untouchedPosition.y;
                            }
                            if (modifyZ)
                            {
                                current_value.z = tweenLinks[Z_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.z = untouchedPosition.z;
                            }
                            break;
                        }
                    case MOD_TYPE.OFFSET:
                        {
                            if (modifyX)
                            {
                                current_value.x = default_value.x + tweenLinks[X_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.x = untouchedPosition.x;
                            }
                            if (modifyY)
                            {
                                current_value.y = default_value.y + tweenLinks[Y_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.y = untouchedPosition.y;
                            }
                            if (modifyZ)
                            {
                                current_value.z = default_value.z + tweenLinks[Z_tweenOutputIndex].value;
                            }
                            else
                            {
                                current_value.z = untouchedPosition.z;
                            }
                            break;
                        }
                }
                ReferenceValue = current_value;
            }
            protected void ConstructorInit(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                modifyX = modify_X;
                modifyY = modify_Y;
                modifyZ = modify_Z;
                modification_type = modification_type_;
                X_tweenOutputIndex = X_tweenOutputIndex_;
                Y_tweenOutputIndex = Y_tweenOutputIndex_;
                Z_tweenOutputIndex = Z_tweenOutputIndex_;
            }

        }
        public abstract class FloatProperty : Property
        {
            float default_value;
            float current_value;
            int tweenOutputIndex;
            protected abstract float ReferenceValue { get; set; }

            public sealed override void Start()
            {
                default_value = ReferenceValue;
            }
            public sealed override void UpdateProperty()
            {

                switch (modification_type)
                {
                    case MOD_TYPE.ABSOLUTE:
                        {
                            current_value = tweenLinks[tweenOutputIndex].value;
                            break;
                        }
                    case MOD_TYPE.OFFSET:
                        {
                            current_value = default_value + tweenLinks[tweenOutputIndex].value;
                            break;
                        }
                }
                ReferenceValue = current_value;
            }
            protected void ConstructorInit(int tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                modification_type = modification_type_;
                tweenOutputIndex = tweenOutputIndex_;
            }

        }
        public abstract class Vector2Property : Property
        {
            Vector2 default_value;
            Vector2 current_value;

            bool modifyX;
            bool modifyY;
            int tweenOutputIndex_X;
            int tweenOutputIndex_Y;
            protected abstract Vector2 ReferenceValue { get; set; }

            public sealed override void Start()
            {
                default_value = ReferenceValue;
            }
            public sealed override void UpdateProperty()
            {
                Vector2 untouchedPosition = ReferenceValue;

                switch (modification_type)
                {
                    case MOD_TYPE.ABSOLUTE:
                        {
                            if (modifyX)
                            {
                                current_value.x = tweenLinks[tweenOutputIndex_X].value;

                            }
                            else
                            {
                                current_value.x = untouchedPosition.x;
                            }
                            if (modifyY)
                            {
                                current_value.y = tweenLinks[tweenOutputIndex_Y].value;

                            }
                            else
                            {
                                current_value.y = untouchedPosition.y;
                            }
                            break;
                        }
                    case MOD_TYPE.OFFSET:
                        {
                            if (modifyX)
                            {
                                current_value.x = default_value.x + tweenLinks[tweenOutputIndex_X].value;

                            }
                            else
                            {
                                current_value.x = untouchedPosition.x;
                            }
                            if (modifyY)
                            {
                                current_value.y = default_value.y + tweenLinks[tweenOutputIndex_Y].value;

                            }
                            else
                            {
                                current_value.y = untouchedPosition.y;
                            }
                            break;
                        }
                }
                ReferenceValue = current_value;
            }
            protected void ConstructorInit(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, MOD_TYPE modification_type_)
            {
                modifyX = modify_X;
                modifyY = modify_Y;
                modification_type = modification_type_;
                tweenOutputIndex_X = tweenOutputIndex_X_;
                tweenOutputIndex_Y = tweenOutputIndex_Y_;
            }

        }
        public abstract class ColorProperty : Property
        {
            Color default_value;
            Color current_value;

            bool modifyR;
            bool modifyG;
            bool modifyB;
            bool modifyA;

            int tweenOutputIndex_R;
            int tweenOutputIndex_G;
            int tweenOutputIndex_B;
            int tweenOutputIndex_A;
            protected abstract Color ReferenceValue { get; set; }

            public sealed override void Start()
            {
                default_value = ReferenceValue;
            }
            public sealed override void UpdateProperty()
            {
                Color untouchedPosition = ReferenceValue;

                switch (modification_type)
                {
                    case MOD_TYPE.ABSOLUTE:
                        {
                            if (modifyR)
                            {
                                current_value.r = tweenLinks[tweenOutputIndex_R].value;
                            }
                            else
                            {
                                current_value.r = untouchedPosition.r;
                            }
                            if (modifyG)
                            {
                                current_value.g = tweenLinks[tweenOutputIndex_G].value;
                            }
                            else
                            {
                                current_value.g = untouchedPosition.g;
                            }
                            if (modifyB)
                            {
                                current_value.b = tweenLinks[tweenOutputIndex_B].value;
                            }
                            else
                            {
                                current_value.b = untouchedPosition.b;
                            }
                            if (modifyA)
                            {
                                current_value.a = tweenLinks[tweenOutputIndex_A].value;
                            }
                            else
                            {
                                current_value.a = untouchedPosition.a;
                            }
                            break;
                        }
                    case MOD_TYPE.OFFSET:
                        {
                            if (modifyR)
                            {
                                current_value.r = default_value.r + tweenLinks[tweenOutputIndex_R].value;
                            }
                            else
                            {
                                current_value.r = untouchedPosition.r;
                            }
                            if (modifyG)
                            {
                                current_value.g = default_value.g + tweenLinks[tweenOutputIndex_G].value;
                            }
                            else
                            {
                                current_value.g = untouchedPosition.g;
                            }
                            if (modifyB)
                            {
                                current_value.b = default_value.b + tweenLinks[tweenOutputIndex_B].value;
                            }
                            else
                            {
                                current_value.b = untouchedPosition.b;
                            }
                            if (modifyA)
                            {
                                current_value.a = default_value.a + tweenLinks[tweenOutputIndex_A].value;
                            }
                            else
                            {
                                current_value.a = untouchedPosition.a;
                            }
                            break;
                        }
                }
                ReferenceValue = current_value;
            }
            protected void ConstructorInit(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
            {
                modifyR = modify_R;
                modifyG = modify_G;
                modifyB = modify_B;
                modifyA = modify_A;
                modification_type = modification_type_;
                tweenOutputIndex_R = tweenOutputIndex_R_;
                tweenOutputIndex_G = tweenOutputIndex_G_;
                tweenOutputIndex_B = tweenOutputIndex_B_;
                tweenOutputIndex_A = tweenOutputIndex_A_;


            }

        }
        public abstract class BoolProperty : Property
        {
            bool current_value;
            bool OnWhenHigherThanLevel;
            float on_off_level;

            int tweenOutputIndex;
            protected abstract bool ReferenceValue { get; set; }

            public sealed override void Start()
            {
            }
            public sealed override void UpdateProperty()
            {
                float value = tweenLinks[tweenOutputIndex].value;

                if (value > on_off_level)
                {
                    current_value = true;
                }
                else
                {
                    current_value = true;
                }

                if (!OnWhenHigherThanLevel)
                    current_value = !current_value;

                ReferenceValue = current_value;

            }
            protected void ConstructorInit(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                OnWhenHigherThanLevel = on_whenHigherThanLevel;
                on_off_level = on_off_level_;
                tweenOutputIndex = tweenOutputIndex_;
            }
        }
        public abstract class IntProperty : Property
        {
            public enum INT_SELECTION_METHOD
            {
                ROUND,
                CEIL,
                FLOOR
            }

            int tweenOutputIndex;
            int current_value;
            int default_value;
            INT_SELECTION_METHOD int_selection_method;
            protected abstract int ReferenceValue { get; set; }

            public sealed override void Start()
            {
                float value = tweenLinks[tweenOutputIndex].value;
                switch (int_selection_method)
                {
                    case INT_SELECTION_METHOD.ROUND: default_value = Mathf.RoundToInt(value); break;
                    case INT_SELECTION_METHOD.FLOOR: default_value = Mathf.FloorToInt(value); break;
                    case INT_SELECTION_METHOD.CEIL: default_value = Mathf.CeilToInt(value); break;
                }
            }
            public sealed override void UpdateProperty()
            {
                float value = tweenLinks[tweenOutputIndex].value;
                int int_value = 0;
                switch (int_selection_method)
                {
                    case INT_SELECTION_METHOD.ROUND: int_value = Mathf.RoundToInt(value); break;
                    case INT_SELECTION_METHOD.FLOOR: int_value = Mathf.FloorToInt(value); break;
                    case INT_SELECTION_METHOD.CEIL: int_value = Mathf.CeilToInt(value); break;
                }


                switch (modification_type)
                {
                    case MOD_TYPE.ABSOLUTE:
                        {
                            current_value = int_value;
                            break;
                        }
                    case MOD_TYPE.OFFSET:
                        {
                            current_value = default_value + int_value;
                            break;
                        }
                }


                ReferenceValue = current_value;

            }
            protected void ConstructorInit(int tweenOutputIndex_, INT_SELECTION_METHOD int_selection_method_, MOD_TYPE modification_type_)
            {
                modification_type = modification_type_;
                int_selection_method = int_selection_method_;
                tweenOutputIndex = tweenOutputIndex_;
            }


        }

        public abstract class TriggerProperty : Property
        {

            public enum TRIGGER_TYPE
            { 
            GREATEREQUAL_THAN,
            LESSEQUAL_THAN,
            FLIP_FLOP            
            }

            TRIGGER_TYPE trigger_type;
            float triggerPoint;
            
            bool flipFlop_TriggerWhenGreater = false;

            int tweenOutputIndex;

            bool isFirstUpdate;

            bool shouldTrigger = false;
            protected abstract void Triggered();
            public sealed override void Start()
            {
                isFirstUpdate = true;
                shouldTrigger = true;
            }
            public sealed override void UpdateProperty()
            {
                float value = tweenLinks[tweenOutputIndex].value;


                switch (trigger_type)
                {
                    case TRIGGER_TYPE.FLIP_FLOP:
                        {
                            if (isFirstUpdate)
                            {
                                isFirstUpdate = false;
                                if (value > triggerPoint)
                                {
                                    flipFlop_TriggerWhenGreater = false;
                                }
                                else
                                {
                                    flipFlop_TriggerWhenGreater = true;
                                }
                            }
                            else
                            {
                                if (flipFlop_TriggerWhenGreater)
                                {
                                    if (value - 0.0001f > triggerPoint)
                                    {
                                        flipFlop_TriggerWhenGreater = !flipFlop_TriggerWhenGreater;
                                        Triggered();
                                    }
                                }
                                else
                                {
                                    if (value + 0.0001f < triggerPoint)
                                    {
                                        flipFlop_TriggerWhenGreater = !flipFlop_TriggerWhenGreater;
                                        Triggered();
                                    }

                                }
                            }
                            break;
                        }
                    case TRIGGER_TYPE.GREATEREQUAL_THAN:
                        {
                            if (shouldTrigger)
                            {
                                if (value >= triggerPoint)
                                {
                                    shouldTrigger = false;
                                    Triggered();
                                }
                            }
                            else
                            {
                                if (value < triggerPoint)
                                {
                                    shouldTrigger = true;
                                }
                            }
                            break;
                        }
                    case TRIGGER_TYPE.LESSEQUAL_THAN:
                        {
                            if (shouldTrigger)
                            {
                                if (value <= triggerPoint)
                                {
                                    shouldTrigger = false;
                                    Triggered();
                                }
                            }
                            else
                            {
                                if (value > triggerPoint)
                                {
                                    shouldTrigger = true;
                                }
                            }
                            break;
                        }
                }

               
            }
            protected void ConstructorInit(int tweenOutputIndex_, float triggerPoint_, TRIGGER_TYPE trigger_type_)
            {
                trigger_type = trigger_type_;
                triggerPoint = triggerPoint_;
                tweenOutputIndex = tweenOutputIndex_;
            }
        }
    }



    public class Generic_ : Animatable
    { 
        public Generic_(TypeRef<Vector3> vector3Reference, Float_ floatType)
        {
            changingProperties.Add(floatType);
            changingProperties[0].Init(vector3Reference);

        }
        public Generic_(TypeRef<Vector2> vector2Reference, Vector2_ vector2Type)
        {
            changingProperties.Add(vector2Type);
            changingProperties[0].Init(vector2Reference);
        }
        public Generic_(TypeRef<float> floatReference, Vector3_ vector3Type)
        {
            changingProperties.Add(vector3Type);
            changingProperties[0].Init(floatReference);
        }
        public Generic_(TypeRef<Color> colorReference, Color_ colourType)
        {
            changingProperties.Add(colourType);
            changingProperties[0].Init(colorReference);
        }
        public Generic_(TypeRef<bool> boolReference, Bool_ boolType)
        {
            changingProperties.Add(boolType);
            changingProperties[0].Init(boolReference);
        }
        public Generic_(TypeRef<int> intReference, Int_ intType)
        {
            changingProperties.Add(intType);
            changingProperties[0].Init(intReference);
        }
        public Generic_(System.Action actionReference, Trigger_ triggerType)
        {
            changingProperties.Add(triggerType);
            changingProperties[0].Init(actionReference);
        }

        public class Float_ : Base.FloatProperty
        {
            public Float_(int tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(tweenOutputIndex_,modification_type);
            }
            protected override float ReferenceValue
            {
                get { return ((TypeRef<float>)reference).value ; }
                set { ((TypeRef<float>)reference).value = value; }
            }

        }
        public class Vector2_ : Base.Vector2Property
        {
            public Vector2_(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, tweenOutputIndex_X_,modify_Y, tweenOutputIndex_Y_, modification_type);
            }
            protected override Vector2 ReferenceValue
            {
                get { return ((TypeRef<Vector2>)reference).value; }
                set { ((TypeRef<Vector2>)reference).value = value; }
            }
        }
        public class Vector3_ : Base.Vector3Property
        {
            public Vector3_(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, bool modify_Z, int tweenOutputIndex_Z_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, tweenOutputIndex_X_, modify_Y, tweenOutputIndex_Y_, modify_Z, tweenOutputIndex_Z_, modification_type);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((TypeRef<Vector3>)reference).value; }
                set { ((TypeRef<Vector3>)reference).value = value; }
            }
        }
        public class Color_ : Base.ColorProperty
        {
            public Color_(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_R, tweenOutputIndex_R_, modify_G, tweenOutputIndex_G_, modify_B, tweenOutputIndex_B_, modify_A, tweenOutputIndex_A_, modification_type_);
            }
            protected override Color ReferenceValue
            {
                get { return ((TypeRef<Color>)reference).value; }
                set { ((TypeRef<Color>)reference).value = value; }
            }
        }
        public class Int_ : Base.IntProperty
        {
            public Int_(int tweenOutputIndex_, INT_SELECTION_METHOD int_selection_method_, MOD_TYPE modification_type_)
            {
                ConstructorInit(tweenOutputIndex_, int_selection_method_, modification_type_);
            }
            protected override int ReferenceValue 
            {
                get { return ((TypeRef<int>)reference).value; }
                set { ((TypeRef<int>)reference).value = value; }
            }
        }
        public class Bool_ : Base.BoolProperty
        {
            public Bool_(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                ConstructorInit(tweenOutputIndex_, on_off_level_, on_whenHigherThanLevel);
            }
            protected override bool ReferenceValue
            {
                get { return ((TypeRef<bool>)reference).value; }
                set { ((TypeRef<bool>)reference).value = value; }
            }
        }
        public class Trigger_ : Base.TriggerProperty
        {
            public Trigger_(int tweenOutputIndex_, float triggerPoint, TRIGGER_TYPE trigger_type)
            {
                ConstructorInit(tweenOutputIndex_, triggerPoint, trigger_type);
            }
            protected override void Triggered()
            {
                ((System.Action)reference)?.Invoke();
            }
        }
    }

    public class Transf_ : Animatable
    {
        public Transf_(UnityEngine.Transform transformReference, Position_ position = null, LocalPosition_ local_position = null, LocalScale_ local_scale = null, LocalRotation_ local_rotation = null, Rotation_ rotation = null ) // add all potential transform options
        {
            if (position != null)
                changingProperties.Add(position);
            if (local_position != null)
                changingProperties.Add(local_position);
            if (local_scale != null)
                changingProperties.Add(local_scale);
            if (local_rotation != null)
                changingProperties.Add(local_rotation);
            if (rotation != null)
                changingProperties.Add(rotation);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(transformReference);
            }
        }

        public class LocalPosition_ : Base.Vector3Property
        {
            public LocalPosition_(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue {
                get { return ((Transform)reference).localPosition; }
                set { ((Transform)reference).localPosition = value; }
                }

        }
        public class Position_ : Base.Vector3Property
        {
            public Position_(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).position; }
                set { ((Transform)reference).position = value; }
            }
        }

        public class LocalScale_ : Base.Vector3Property
        {
            public LocalScale_(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).localScale; }
                set { ((Transform)reference).localScale = value; }
            }
        }

        public class LocalRotation_ : Base.Vector3Property
        {
            public LocalRotation_(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).localRotation.eulerAngles; }
                set { ((Transform)reference).localRotation = Quaternion.Euler(value); }
            }
        }
        public class Rotation_ : Base.Vector3Property
        {
            public Rotation_(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).rotation.eulerAngles; }
                set { ((Transform)reference).rotation = Quaternion.Euler(value); }
            }
        }
    }

    public class TransfRect_ : Animatable
    {
        public TransfRect_(UnityEngine.RectTransform rectTransformReference, AnchorPosition_ anchor_position = null, Scale_ scale = null, Rotation_ rotation = null ) // add all potential transform options
        {
            if (anchor_position != null)
                changingProperties.Add(anchor_position);
            if (scale != null)
                changingProperties.Add(scale);
            if (rotation != null)
                ChangingProperties.Add(rotation);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(rectTransformReference);
            }
        }

        public class AnchorPosition_ : Base.Vector2Property
        {
            public AnchorPosition_(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, tweenOutputIndex_X_, modify_Y, tweenOutputIndex_Y_, modification_type_);
            }
            protected override Vector2 ReferenceValue
            {
                get { return ((RectTransform)reference).anchoredPosition; }
                set { ((RectTransform)reference).anchoredPosition = value; }
            }
        }

        public class Scale_ : Base.Vector2Property
        {
            Vector3 _3Dversion = new Vector3(0,0,1);
            public Scale_(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, tweenOutputIndex_X_, modify_Y, tweenOutputIndex_Y_, modification_type_);
            }
            protected override Vector2 ReferenceValue
            {
                get { return ((RectTransform)reference).localScale; }
                set 
                {
                    _3Dversion.x = value.x;
                    _3Dversion.y = value.y;
                    ((RectTransform)reference).localScale = _3Dversion;                 
                }
            }
        }
        public class Rotation_ : Base.FloatProperty
        {
            Vector3 _3Dversion = new Vector3(0, 0, 0);
            Quaternion quaternion = new Quaternion();
            public Rotation_(int outputIndex, MOD_TYPE modification_type_)
            {
                ConstructorInit(outputIndex, modification_type_);
            }
            protected override float ReferenceValue
            {
                get { return ((RectTransform)reference).localRotation.eulerAngles.z; }
                set
                {
                    _3Dversion.z = value;
                    quaternion.eulerAngles = _3Dversion;
                    ((RectTransform)reference).localRotation = quaternion;
                }
            }
        }
    }

    public class TMPText_ : Animatable
    {
        public TMPText_(TMPro.TextMeshProUGUI textReference, Color_ color = null) // add all potential transform options
        {
            if (color != null)
                changingProperties.Add(color);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(textReference);
            }
        }

        public class Color_ : Base.ColorProperty
        {
            public Color_(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_R, tweenOutputIndex_R_, modify_G, tweenOutputIndex_G_, modify_B, tweenOutputIndex_B_, modify_A, tweenOutputIndex_A_, modification_type_);
            }
            protected override UnityEngine.Color ReferenceValue
            {
                get { return ((TMPro.TextMeshProUGUI)reference).color; }
                set { ((TMPro.TextMeshProUGUI)reference).color = value; }
            }
        }
    }
    public class Image_ : Animatable
    {
        public Image_(UnityEngine.UI.Image imageReference, Color_ color = null) // add all potential transform options
        {
            if (color != null)
                changingProperties.Add(color);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(imageReference);
            }
        }

        public class Color_ : Base.ColorProperty
        {
            public Color_(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_R, tweenOutputIndex_R_, modify_G, tweenOutputIndex_G_, modify_B, tweenOutputIndex_B_, modify_A, tweenOutputIndex_A_, modification_type_);
            }
            protected override UnityEngine.Color ReferenceValue
            {
                get { return ((UnityEngine.UI.Image)reference).color; }
                set { ((UnityEngine.UI.Image)reference).color = value; }
            }
        }


    }

    public class HDRP_Animator_ : Animatable
    {
        public HDRP_Animator_(HDRP_Unlit_ManualAnimator animatorReference, Frame_ frame = null, FlipX_ flipX = null, FlipY_ flipY = null)
        {
            if (frame != null)
                changingProperties.Add(frame);
            if (flipX != null)
                changingProperties.Add(flipX);
            if (flipY != null)
                changingProperties.Add(flipY);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(animatorReference);
            }
        }

        public class Frame_ : Base.IntProperty
        {
            public Frame_(int tweenOutputIndex_, INT_SELECTION_METHOD int_selection_method_, MOD_TYPE modification_type_)
            {
                ConstructorInit(tweenOutputIndex_, int_selection_method_, modification_type_);
            }
            protected override int ReferenceValue
            {
                get { return ((HDRP_Unlit_ManualAnimator)reference).CurrentFrame; }
                set { ((HDRP_Unlit_ManualAnimator)reference).SetFrame(value); }
            }
        }
        public class FlipX_ : Base.BoolProperty
        {
            public FlipX_(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                ConstructorInit(tweenOutputIndex_, on_off_level_, on_whenHigherThanLevel);
            }
            protected override bool ReferenceValue
            {
                get { return ((HDRP_Unlit_ManualAnimator)reference).FlipX; }
                set { ((HDRP_Unlit_ManualAnimator)reference).FlipX = value; }
            }
        }
        public class FlipY_ : Base.BoolProperty
        {
            public FlipY_(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                ConstructorInit(tweenOutputIndex_, on_off_level_, on_whenHigherThanLevel);
            }
            protected override bool ReferenceValue
            {
                get { return ((HDRP_Unlit_ManualAnimator)reference).FlipY; }
                set { ((HDRP_Unlit_ManualAnimator)reference).FlipY = value; }
            }
        }
    }

    public class HDRP_Unlit_Material_ : Animatable
    {
        public HDRP_Unlit_Material_(Material materialReference, Color_ color = null)
        {
            if (color != null)
                changingProperties.Add(color);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(materialReference);
            }
        }
        public class Color_ : Base.ColorProperty
        {
            static int colourProperyID = Shader.PropertyToID("_UnlitColor");
            public Color_(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_R, tweenOutputIndex_R_, modify_G, tweenOutputIndex_G_, modify_B, tweenOutputIndex_B_, modify_A, tweenOutputIndex_A_, modification_type_);
            }
            protected override UnityEngine.Color ReferenceValue
            {
                get { return ((Material)reference).GetColor(colourProperyID); }
                set { ((Material)reference).SetColor(colourProperyID, value); }
            }
        }
    }



    public class AudioSFXInstanceInterface_ : Animatable
    {
        public AudioSFXInstanceInterface_(AudioManager.SFXInstanceInterface instanceInterfaceReference, Loop_ loop = null, Mute_ mute = null, Pitch_ pitch = null, SpatialBlend_ spatialBlend = null, Panning_ panning = null, Priority_ priority = null, Volume_ volume = null )
        {
            if (loop != null)
                changingProperties.Add(loop);
            if (mute != null)
                changingProperties.Add(mute);
            if (pitch != null)
                changingProperties.Add(pitch);
            if (spatialBlend != null)
                changingProperties.Add(spatialBlend);
            if (panning != null)
                changingProperties.Add(panning);
            if (priority != null)
                changingProperties.Add(priority);
            if (volume != null)
                changingProperties.Add(volume);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(instanceInterfaceReference);
            }
        }

        public class Loop_ : Base.BoolProperty
        {
            public Loop_(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                ConstructorInit(tweenOutputIndex_, on_off_level_, on_whenHigherThanLevel);
            }
            protected override bool ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Loop; }
                set { ((AudioManager.SFXInstanceInterface)reference).Loop = value; }
            }

        }

        public class Mute_ : Base.BoolProperty
        {
            public Mute_(int tweenOutputIndex_, float on_off_level_, bool on_whenHigherThanLevel)
            {
                ConstructorInit(tweenOutputIndex_, on_off_level_, on_whenHigherThanLevel);
            }
            protected override bool ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Mute; }
                set { ((AudioManager.SFXInstanceInterface)reference).Mute = value; }
            }

        }

        public class Pitch_ : Base.FloatProperty
        {
            public Pitch_(int tweenOutputIndex_)
            {
                ConstructorInit(tweenOutputIndex_, MOD_TYPE.ABSOLUTE);
            }
            protected override float ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Pitch; }
                set { ((AudioManager.SFXInstanceInterface)reference).Pitch = value; }
            }
        }

        public class SpatialBlend_ : Base.FloatProperty
        {
            public SpatialBlend_(int tweenOutputIndex_)
            {
                ConstructorInit(tweenOutputIndex_, MOD_TYPE.ABSOLUTE);
            }
            protected override float ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).SpatialBlend; }
                set { ((AudioManager.SFXInstanceInterface)reference).SpatialBlend = value; }
            }

        }

        public class Panning_ : Base.FloatProperty
        {
            public Panning_(int tweenOutputIndex_)
            {
                ConstructorInit(tweenOutputIndex_, MOD_TYPE.ABSOLUTE);
            }
            protected override float ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Panning; }
                set { ((AudioManager.SFXInstanceInterface)reference).Panning = value; }
            }
        }
        public class Priority_ : Base.IntProperty
        {
            public Priority_(int tweenOutputIndex_, INT_SELECTION_METHOD int_selection_method_)
            {
                ConstructorInit(tweenOutputIndex_, int_selection_method_, MOD_TYPE.ABSOLUTE);
            }
            protected override int ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Priority; }
                set { ((AudioManager.SFXInstanceInterface)reference).Priority = value; }
            }
        }

        public class Volume_ : Base.FloatProperty
        {
            public Volume_(int tweenOutputIndex_)
            {
                ConstructorInit(tweenOutputIndex_, MOD_TYPE.ABSOLUTE);
            }
            protected override float ReferenceValue
            {
                get { return ((AudioManager.SFXInstanceInterface)reference).Volume; }
                set { ((AudioManager.SFXInstanceInterface)reference).Volume = value; }
            }
        }
    }


    public class SpecialText_ : Animatable
    {
        public SpecialText_(SpecialText.SpecialText specialTextReference, Begin_ beginCall = null, End_ endCall = null, ForceAll_ forceAllCall = null, Revert_ revertCall = null, Hide_ hideCall = null)
        {
            if (beginCall != null)
                changingProperties.Add(beginCall);
            if (endCall != null)
                changingProperties.Add(endCall);
            if (forceAllCall != null)
                changingProperties.Add(forceAllCall);
            if (revertCall != null)
                changingProperties.Add(revertCall);
            if (hideCall != null)
                changingProperties.Add(hideCall);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(specialTextReference);
            }

        }
        public class Begin_ : Base.TriggerProperty
        {
            SpecialText.SpecialTextData arg1;
            System.Action arg2;
            public Begin_(int tweenOutputIndex_, float triggerPoint,TRIGGER_TYPE triggerType, SpecialText.SpecialTextData arg1_SpecialTextData_, System.Action arg2_textCompleted_ = null)
            {
                arg1 = arg1_SpecialTextData_;
                arg2 = arg2_textCompleted_;
                ConstructorInit(tweenOutputIndex_, triggerPoint, triggerType);
            }
            protected override void Triggered()
            {
                ((SpecialText.SpecialText)reference).Begin(arg1, arg2);
            }
        }

        public class End_ : Base.TriggerProperty
        {
            public End_(int tweenOutputIndex_, float triggerPoint, TRIGGER_TYPE triggerType)
            {
                ConstructorInit(tweenOutputIndex_, triggerPoint, triggerType);
            }
            protected override void Triggered()
            {
                ((SpecialText.SpecialText)reference).End();
            }
        }
        public class ForceAll_ : Base.TriggerProperty
        {
            public ForceAll_(int tweenOutputIndex_, float triggerPoint, TRIGGER_TYPE triggerType)
            {
                ConstructorInit(tweenOutputIndex_, triggerPoint, triggerType);
            }
            protected override void Triggered()
            {
                ((SpecialText.SpecialText)reference).ForceAll();
            }
        }
        public class Revert_ : Base.TriggerProperty
        {
            public Revert_(int tweenOutputIndex_, float triggerPoint, TRIGGER_TYPE triggerType)
            {
                ConstructorInit(tweenOutputIndex_, triggerPoint, triggerType);
            }
            protected override void Triggered()
            {
                ((SpecialText.SpecialText)reference).Revert();
            }
        }
        public class Hide_ : Base.TriggerProperty
        {
            public Hide_(int tweenOutputIndex_, float triggerPoint, TRIGGER_TYPE triggerType)
            {
                ConstructorInit(tweenOutputIndex_, triggerPoint, triggerType);
            }
            protected override void Triggered()
            {
                ((SpecialText.SpecialText)reference).Hide();
            }
        }
    }
}


