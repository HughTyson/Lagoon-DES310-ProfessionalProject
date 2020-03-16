using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager
{
    List<TweenInstance> currentTweens = new List<TweenInstance>();


    public TweenManager()
    {

    }


    /// <summary>
    /// The curve of the tween segment
    /// </summary>
    public enum CURVE_PRESET
    {
        /// <summary> move from start to end at a constant speed</summary>
        LINEAR,
        /// <summary> move from start to end with an accelleration</summary>
        EASE_IN,
        /// <summary> move from start to end with a decelleration</summary>
        EASE_OUT,
        /// <summary> move from start to end with an accelleration then a decelleration</summary>
        EASE_INOUT,
        /// <summary> move from start to end with a decelleration then an accelleration</summary>
        NON_EASE_INOUT
    }

    public enum DIRECTION
    {
        START_TO_END = 1,
        END_TO_START = -1
    }
    /// <summary>
    /// Stops the tweening. Fires the TweenComplete delegate once stopped.
    /// </summary>
    public enum STOP_COMMAND
    {
        /// <summary> Immediatly stop the tween, leaving the values where they are in the moment.</summary>
        IMMEDIATE,
        /// <summary> Transition from current values to end values. Note: if tween is moving in opposite direction, the change in direction will be noticable. If this is unwanted, use SEEMLESS_TO_END</summary>
        TRANSITION_TO_END,
        /// <summary> Transition from current values to start values. Note: if tween is moving in opposite direction, the change in direction will be noticable. If this is unwanted, use SEEMLESS_TO_START</summary>
        TRANSITION_TO_START,
        /// <summary> Transition from current values to the final values of the tweening direction</summary>
        SEEMLESS,
        /// <summary> Transition from current values to the opposite values of the tweening direction</summary>
        INVERSE_SEEMLESS,
        /// <summary> Transition from current values to the final values of the tweening direction, then (unless at end point), transition to end point</summary>
        SEEMLESS_TO_END,
        /// <summary> Transition from current values to the final values of the tweening direction, then (unless at start point), transition to start point</summary>
        SEEMLESS_TO_START
    };

    ///<summary>
    /// The logic of the full tween.
    /// </summary>
    public enum PATH
    {
        ///<summary>Go from start to end.</summary>
        NORMAL,
        ///<summary>Go from start to end to start until told to stop using a STOP_COMMAND.</summary>
        PING_PONG
    }


    public enum TIME_FORMAT
    {
        DELTA,
        FIXED_DELTA,
        UNSCALE_DELTA,
        UNSCALED_FIXED_DELTA
    }

    /// <summary>
    /// Only call in GAME_MANAGER.
    /// </summary>
    public void Update()
    {
        int count = currentTweens.Count;
        for (int i = count - 1; i >= 0; i--)
        {
            if (currentTweens[i].Update())
            {
                currentTweens[i].actionTweenComplete?.Invoke();
                currentTweens.RemoveAt(i);
            }
        }
    }


    /// <summary>
    /// Create a self-dependant tween
    /// </summary>
    /// <returns>Returns an optional commander delgate. Allows the stopping of a tween by force</returns>
    public System.Action<STOP_COMMAND> StartTweenInstance(TweenPathBundle pathBundle_, TypeRef<float>[] valueRefs_,  System.Action tweenUpdatedDelegate_ = null, System.Action tweenCompleteDelegate_ = null, PATH path_ = PATH.NORMAL, DIRECTION startingDirection_ = DIRECTION.START_TO_END, TIME_FORMAT TimeFormat_ = TIME_FORMAT.DELTA, float speed_ = 1)
    {
        TweenInstance instance = new TweenInstance(pathBundle_, tweenUpdatedDelegate_, tweenCompleteDelegate_, path_, startingDirection_, TimeFormat_, speed_, valueRefs_);
        currentTweens.Add(instance);
        instance.Update();
        return instance.Command;
    }


    class TweenInstance
    {
        TypeRef<float>[] valueRefs;
        public readonly System.Action actionTweenComplete;
        public readonly System.Action actionTweenUpdated;
        TIME_FORMAT timeFormat;
        TweenPathBundle pathBundle;
        float current_time;
        float duration;
        float speed;
        DIRECTION direction;
        PATH tween_path;
        bool isStopping = false;
        STOP_COMMAND stop_command;


       


        public TweenInstance(TweenPathBundle pathBundle_,System.Action actionTweenUpdated_, System.Action actionTweenComplete_, PATH tween_path_, DIRECTION starting_direction = DIRECTION.START_TO_END, TIME_FORMAT time_format_ = TIME_FORMAT.DELTA, float speed_ = 1, params TypeRef<float>[] valueRefs_)
        {
            actionTweenComplete = actionTweenComplete_;
            actionTweenUpdated = actionTweenUpdated_;

            timeFormat = time_format_;
            pathBundle = pathBundle_;
            duration = pathBundle_.Duration;

            direction = starting_direction;
            if (direction == DIRECTION.START_TO_END)
                current_time = 0;
            else if (direction == DIRECTION.END_TO_START)
                current_time = duration;

            tween_path = tween_path_;
            speed = speed_;
            valueRefs = valueRefs_;
        }

        public bool Update()
        {
            float time;
            switch (timeFormat)
            {
                case TIME_FORMAT.DELTA: time = Time.deltaTime; break;
                case TIME_FORMAT.FIXED_DELTA: time = Time.fixedDeltaTime; break;
                case TIME_FORMAT.UNSCALED_FIXED_DELTA: time = Time.fixedUnscaledDeltaTime; break;
                case TIME_FORMAT.UNSCALE_DELTA: time = Time.unscaledDeltaTime; break;
                default: time = 0; break;
            }
            time *= (float)direction;
            current_time += time * speed;
            pathBundle.GetValues(current_time, valueRefs);
            actionTweenUpdated?.Invoke();

            if (isStopping)
            {
                switch(stop_command)
                {
                    case STOP_COMMAND.IMMEDIATE:
                        {

                            return true;
                        }
                    case STOP_COMMAND.INVERSE_SEEMLESS:
                        {
                            switch (direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                            return true;
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                            return true;
                                        break;
                                    }
                            }
                            break;
                        }
                    case STOP_COMMAND.SEEMLESS:
                        {
                            switch (direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                            return true;
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                            return true;
                                        break;
                                    }
                            }
                            break;
                        }
                    case STOP_COMMAND.SEEMLESS_TO_END:
                        {
                            switch (direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                            return true;
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                            direction = DIRECTION.START_TO_END;
                                        break;
                                    }
                            }
                            break;
                        }
                    case STOP_COMMAND.SEEMLESS_TO_START:
                        {
                            switch (direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                            direction = DIRECTION.END_TO_START;
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                            return true;
                                        break;
                                    }
                            }
                            break;
                        }
                    case STOP_COMMAND.TRANSITION_TO_END:
                        {
                            if (current_time > duration)
                                return true;
                            break;
                        }
                    case STOP_COMMAND.TRANSITION_TO_START:
                        {
                            if (current_time < 0)
                                return true;
                            break;
                        }
                }
            }
            else
            {
                switch (tween_path)
                {
                        case PATH.NORMAL:
                        {
                            switch (direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                        {
                                            return true;
                                        }
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                        {
                                            return true;
                                        }
                                        break;
                                    }
                            }
                            break;
                        }
                        case PATH.PING_PONG:
                        {
                            switch(direction)
                            {
                                case DIRECTION.START_TO_END:
                                    {
                                        if (current_time > duration)
                                            direction = DIRECTION.END_TO_START;
                                        break;
                                    }
                                case DIRECTION.END_TO_START:
                                    {
                                        if (current_time < 0)
                                            direction = DIRECTION.START_TO_END;
                                        break;
                                    }
                            }
                            break;
                        }     
                    }
                }
            return false;

        }

        public void Command(STOP_COMMAND cmd)
        {
            isStopping = true;
            stop_command = cmd;

            switch (stop_command)
            {
                case STOP_COMMAND.INVERSE_SEEMLESS:
                    {
                        switch (direction)
                        {
                            case DIRECTION.START_TO_END:
                                {
                                    direction = DIRECTION.END_TO_START;
                                    break;
                                }
                            case DIRECTION.END_TO_START:
                                {
                                    direction = DIRECTION.START_TO_END;
                                    break;
                                }
                        }
                        break;
                    }
                case STOP_COMMAND.TRANSITION_TO_END:
                    {
                        direction = DIRECTION.START_TO_END;
                        break;
                    }
                case STOP_COMMAND.TRANSITION_TO_START:
                    {
                        direction = DIRECTION.END_TO_START;
                        break;
                    }
            }
        }
    }   



    /// <summary>
    /// A complete tween. Allows the grouping of multiple composite tweens.
    /// </summary>
    public class TweenPathBundle
    {
        List<TweenPath> compositeTweens = new List<TweenPath>();
        float duration = 0;

        public float Duration { get { return duration; } }
        public TweenPathBundle(params TweenPath[] composites)
        {
            for (int i = 0; i < composites.Length; i++)
            {
                compositeTweens.Add(composites[i]);
                duration = Mathf.Max(composites[i].TotalDuration, duration);
            }
            for (int i = 0; i < composites.Length; i++)
            {
                composites[i].ApplyFiller(duration);
            }
        }
        public void GetValues(float time ,params TypeRef<float>[] valRefs)
        {
            for (int i = 0; i < valRefs.Length; i++)
            {
                valRefs[i].value = compositeTweens[i].GetValue(time);
            }

        }
    }


    


    /// <summary>
    /// 1D composite tween. A composite tween is the full path of multiple connected tweens
    /// </summary>
    public class TweenPath
    {
        float totalDuration = 0;
        public float TotalDuration {get {return totalDuration;} }
        List<TweenPart_Start> tweenOrder = new List<TweenPart_Start>();
        public TweenPath(TweenPart_Start start, params TweenPart_Continue[] tweens)
        {
            tweenOrder.Add(start);
            totalDuration += start.Duration;
            for (int i = 0; i < tweens.Length; i++)
            {
                tweens[i].FillMissedInfo(tweenOrder[tweenOrder.Count - 1]);
                tweenOrder.Add(tweens[i]);
                totalDuration += tweens[i].Duration;
            }
        }

        
        public float GetValue(float time)
        {
            float duration = time;
           for (int i = 0; i < tweenOrder.Count; i++)
            {
                if (tweenOrder[i].Duration > duration)
                {
                    return tweenOrder[i].GetValue(duration);
                }
                else
                {
                    duration -= tweenOrder[i].Duration;
                }
            }
            return tweenOrder[tweenOrder.Count - 1].GetValue(tweenOrder[tweenOrder.Count - 1].Duration);
        }

        public void ApplyFiller(float desiredLength)
        {
            if (totalDuration < desiredLength)
            {
                tweenOrder.Add(new TweenPart_Delay(desiredLength - totalDuration));
                tweenOrder[tweenOrder.Count - 1].FillMissedInfo(tweenOrder[tweenOrder.Count - 2]);
                totalDuration = desiredLength;
            }
        }

    }


    public class TweenPart_Start
    {
        protected float start;
        protected float end;
        protected float duration;
        protected AnimationCurve curve = null;
        protected CURVE_PRESET preset;
        protected bool inverse_curve = false;
        

        public float Start { get { return start; } }
        public float End { get { return end; } }

        public float Duration { get { return duration; } }

        protected TweenPart_Start() { }
        public TweenPart_Start(float start_, float end_, float duration_, CURVE_PRESET preset_)
        {
            start = start_;
            end = end_;
            duration = duration_;
            preset = preset_;
        }
        public TweenPart_Start(float start_, float end_, float duration_, AnimationCurve curve_, bool inverse_curve_ = false)
        {
            start = start_;
            end = end_;
            duration = duration_;
            curve = curve_;
            inverse_curve = inverse_curve_;
        }


        public virtual float GetValue(float time)
        {
            float t = Mathf.Clamp01(time / duration);
            if (curve == null)
            {
                switch (preset)
                {
                    case CURVE_PRESET.LINEAR:
                        {
                            break;
                        }
                    case CURVE_PRESET.EASE_IN:
                        {
                            t = Transformations1D.EaseIn(t);
                            break;
                        }
                    case CURVE_PRESET.EASE_OUT:
                        {
                            t = Transformations1D.EaseOut(t);
                            break;
                        }
                    case CURVE_PRESET.EASE_INOUT:
                        {
                            t = Transformations1D.EaseInOut(t);
                            break;
                        }
                    case CURVE_PRESET.NON_EASE_INOUT:
                        {
                            t = Transformations1D.NotEaseInOut(t);
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                if (inverse_curve)
                {
                    t = (1.0f - curve.Evaluate(1.0f - t));
                }
                else
                {
                    t = curve.Evaluate(t);
                }
               
            }

            return Mathf.LerpUnclamped(start, end, t);          
        }
        
        public virtual void FillMissedInfo(TweenPart_Start referenceTween)
        {

            
        }
    }

    public class TweenPart_Continue : TweenPart_Start
    { 
        protected TweenPart_Continue() { }
        public TweenPart_Continue(float end_, float duration_, CURVE_PRESET preset_)
        {
            end = end_;
            duration = duration_;
            preset = preset_;
        }
        public TweenPart_Continue(float end_, float duration_, AnimationCurve curve_)
        {
            end = end_;
            duration = duration_;
            curve = curve_;
        }
        public override void FillMissedInfo(TweenPart_Start referenceTween)
        {
            start = referenceTween.End;
        }
    }
    public class TweenPart_Delay : TweenPart_Continue
    {   
        public TweenPart_Delay(float duration_)
        {
            duration = duration_;
        }
        
        public override float GetValue(float time)
        {
            return start;
        }
        public override void FillMissedInfo(TweenPart_Start referenceTween)
        {
            start = referenceTween.End;
            end = referenceTween.End;
        }
    }


}
