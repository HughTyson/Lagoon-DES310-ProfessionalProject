using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimator
{
    public class Animation
    {
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

        BaseProperty[] changingProperties;

        TypeRef<float>[] tweenOutputs;

        TweenManager.TweenPathBundle tweenBundle;

        System.Action animationUpdatedDelegate;
        System.Action animationCompleteDelegate;

        public Animation(TweenManager.TweenPathBundle tweenBundle_, params Animatable[] animatables_)
        {
            tweenBundle = tweenBundle_;

            List<BaseProperty> changingPropertiesList = new List<BaseProperty>();
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
        public void StopAnimation(TweenManager.STOP_COMMAND stopCommand)
        {
            if (tweenInterface.Exists)
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
        protected List<BaseProperty> changingProperties = new List<BaseProperty>();
        public List<BaseProperty> ChangingProperties => changingProperties;
    }

    public enum MOD_TYPE
    { 
    ABSOLUTE,
    OFFSET
    }

    public abstract class BaseProperty
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
    public abstract class BaseVector3Property : BaseProperty
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
    public abstract class BaseFloatProperty : BaseProperty
    {
        float default_value;
        float current_value;
        int tweenOutputIndex;
        protected abstract float ReferenceValue {get; set;}

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
    public abstract class BaseVector2Property : BaseProperty
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
    public abstract class BaseColorProperty : BaseProperty
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
            tweenOutputIndex_G   = tweenOutputIndex_G_;
            tweenOutputIndex_B   = tweenOutputIndex_B_;
            tweenOutputIndex_A   = tweenOutputIndex_A_;


        }

    }

    public abstract class BaseBoolProperty : BaseProperty
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
    public abstract class BaseIntProperty : BaseProperty
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
                case INT_SELECTION_METHOD.CEIL:  int_value = Mathf.CeilToInt(value); break;
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

        public class Float_ : BaseFloatProperty
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
        public class Vector2_ : BaseVector2Property
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
        public class Vector3_ : BaseVector3Property
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
        public class Color_ : BaseColorProperty
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
        public class Int_ : BaseIntProperty
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
        public class Bool_ : BaseBoolProperty
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

    }

    public class Transf_ : Animatable
    {
        public Transf_(UnityEngine.Transform transformReference, Position position = null, LocalPosition local_position = null, LocalScale local_scale = null, LocalRotation local_rotation = null, Rotation rotation = null ) // add all potential transform options
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

        public class LocalPosition : BaseVector3Property
        {
            public LocalPosition(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue {
                get { return ((Transform)reference).localPosition; }
                set { ((Transform)reference).localPosition = value; }
                }

        }
        public class Position : BaseVector3Property
        {
            public Position(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).position; }
                set { ((Transform)reference).position = value; }
            }
        }

        public class LocalScale : BaseVector3Property
        {
            public LocalScale(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).localScale; }
                set { ((Transform)reference).localScale = value; }
            }
        }

        public class LocalRotation : BaseVector3Property
        {
            public LocalRotation(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, X_tweenOutputIndex_, modify_Y, Y_tweenOutputIndex_, modify_Z, Z_tweenOutputIndex_, modification_type_);
            }
            protected override Vector3 ReferenceValue
            {
                get { return ((Transform)reference).localRotation.eulerAngles; }
                set { ((Transform)reference).localRotation = Quaternion.Euler(value); }
            }
        }
        public class Rotation : BaseVector3Property
        {
            public Rotation(bool modify_X, int X_tweenOutputIndex_, bool modify_Y, int Y_tweenOutputIndex_, bool modify_Z, int Z_tweenOutputIndex_, MOD_TYPE modification_type_)
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
        public TransfRect_(UnityEngine.RectTransform rectTransformReference, AnchorPosition anchor_position = null ) // add all potential transform options
        {
            if (anchor_position != null)
                changingProperties.Add(anchor_position);

            for (int i = 0; i < changingProperties.Count; i++)
            {
                changingProperties[i].Init(rectTransformReference);
            }
        }

        public class AnchorPosition : BaseVector2Property
        {
            public AnchorPosition(bool modify_X, int tweenOutputIndex_X_, bool modify_Y, int tweenOutputIndex_Y_, MOD_TYPE modification_type_)
            {
                ConstructorInit(modify_X, tweenOutputIndex_X_, modify_Y, tweenOutputIndex_Y_, modification_type_);
            }
            protected override Vector2 ReferenceValue
            {
                get { return ((RectTransform)reference).anchoredPosition; }
                set { ((RectTransform)reference).anchoredPosition = value; }
            }
        }
    }

    public class TMPText_ : Animatable
    {
        public class Color : BaseColorProperty
        {
            public Color(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
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
        public class Color : BaseColorProperty
        {
            public Color(bool modify_R, int tweenOutputIndex_R_, bool modify_G, int tweenOutputIndex_G_, bool modify_B, int tweenOutputIndex_B_, bool modify_A, int tweenOutputIndex_A_, MOD_TYPE modification_type_)
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

}


