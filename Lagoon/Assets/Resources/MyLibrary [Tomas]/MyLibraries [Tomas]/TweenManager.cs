using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenManager
{
    List<FullTween> currentTweens = new List<FullTween>();

    public enum TRANSF
    { 
    LINEAR,
    EASE_IN,
    EASE_OUT,
    EASE_INOUT,
    NON_EASE_INOUT    
    }

    public void Update()
    {
        currentTweens.RemoveAll(y =>
        {         
            return y.Update();
        }
        );
    }

    public void CreateTween(FullTween fullTween)
    {
        currentTweens.Add(fullTween);
    }


    public class FullTween
    {
        List<CompositeTween<BaseTween>> compositeTweens = new List<CompositeTween<BaseTween>>();
        System.Action actionTweenComplete;
        public FullTween(System.Action actionTweenComplete_ ,params CompositeTween<BaseTween>[] composites)
        {
            actionTweenComplete = actionTweenComplete_;
            for (int i = 0; i < composites.Length; i++)
            {
                compositeTweens.Add(composites[i]);
            }
        }
        public bool Update()
        {
            compositeTweens.RemoveAll(y =>
            {
               return y.Update();
            }
            );
            if (compositeTweens.Count == 0)
            {
                actionTweenComplete?.Invoke();
                return true;
            }
            return false;
        }
        
    
    }

    public class CompositeTween<T> where T : BaseTween
    {
        Queue<T> tweenQueue = new Queue<T>();
        public CompositeTween(params T[] tweens)
        {
            for (int i = 0; i < tweens.Length; i++)
            {
                tweenQueue.Enqueue(tweens[i]);
            }
        }
        public bool Update()
        {
            if (tweenQueue.Peek().Update())
            {
                tweenQueue.Dequeue();
                if (tweenQueue.Count == 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
    abstract public class BaseTween
    {
        protected float CalculateTween(TRANSF transf,float t)
        {
            switch(transf)
            {
                case TRANSF.LINEAR:
                    {
                        return t;
                    }
                case TRANSF.EASE_IN:
                    {
                        return Transformations1D.EaseIn(t);                        
                    }
                case TRANSF.EASE_OUT:
                    {
                        return Transformations1D.EaseOut(t);
                    }
                case TRANSF.EASE_INOUT:
                    {
                        return Transformations1D.EaseInOut(t);
                    }
                case TRANSF.NON_EASE_INOUT:
                    {
                        return Transformations1D.NotEaseInOut(t);
                    }
                default:
                    {
                        return t;
                    }
            }
        }
        public abstract bool Update();
    }
    public class Tween1D : BaseTween
    {
        TypeRef<float> outputRef;

        float start;
        float end;
        float duration;
        TRANSF transf;
        float current_time;
        public Tween1D(TypeRef<float> outputRef_, float start_, float end_, float duration_, TRANSF transf_)
        {
            outputRef = outputRef_;
            start = start_;
            end = end_;
            duration = duration_;
            transf = transf_;
            current_time = 0;
        }
        public override bool Update()
        {
            current_time += Time.deltaTime;
            outputRef.value = Mathf.LerpUnclamped(start, end, CalculateTween(transf, Mathf.Clamp01(current_time/duration)));
            if (current_time > duration)
            {
                return true;
            }
            return false;
        }
    }
    public class Tween2D : BaseTween
    {
        TypeRef<Vector2> outputRef;

        Vector2 start;
        Vector2 end;
        Vector2 duration;
        System.Tuple<TRANSF, TRANSF> transf;
        Vector2 current_time;

        public Tween2D(TypeRef<Vector2> outputRef_, Vector2 start_, Vector2 end_, Vector2 duration_, System.Tuple<TRANSF,TRANSF> transf_)
        {
            outputRef = outputRef_;
            start = start_;
            end = end_;
            duration = duration_;
            transf = transf_;
            current_time = Vector2.zero;
        }
        public override bool Update()
        {
            current_time.x += Time.deltaTime;
            current_time.y += Time.deltaTime;

            outputRef.value.x = Mathf.LerpUnclamped(start.x, end.x, CalculateTween(transf.Item1, Mathf.Clamp01(current_time.x / duration.x)));
            outputRef.value.y = Mathf.LerpUnclamped(start.y, end.y, CalculateTween(transf.Item2, Mathf.Clamp01(current_time.y / duration.y)));
            if (current_time.x > duration.x && current_time.y > duration.y)
            {
                return true;
            }
            return false;
        }
    }
    public class Tween3D : BaseTween
    {
        TypeRef<Vector3> outputRef;

        Vector3 start;
        Vector3 end;
        Vector3 duration;
        System.Tuple<TRANSF, TRANSF, TRANSF> transf;
        Vector3 current_time;

        public Tween3D(TypeRef<Vector3> outputRef_, Vector3 start_, Vector3 end_, Vector3 duration_, System.Tuple<TRANSF, TRANSF, TRANSF> transf_)
        {
            outputRef = outputRef_;
            start = start_;
            end = end_;
            duration = duration_;
            transf = transf_;
            current_time = Vector2.zero;
        }
        public override bool Update()
        {
            current_time.x += Time.deltaTime;
            current_time.y += Time.deltaTime;
            current_time.z += Time.deltaTime;

            outputRef.value.x = Mathf.LerpUnclamped(start.x, end.x, CalculateTween(transf.Item1, Mathf.Clamp01(current_time.x / duration.x)));
            outputRef.value.y = Mathf.LerpUnclamped(start.y, end.y, CalculateTween(transf.Item2, Mathf.Clamp01(current_time.y / duration.y)));
            outputRef.value.z = Mathf.LerpUnclamped(start.z, end.z, CalculateTween(transf.Item2, Mathf.Clamp01(current_time.z / duration.z)));
            if (current_time.x > duration.x && current_time.y > duration.y && current_time.z > duration.z)
            {
                return true;
            }
            return false;
        }
    }
}
