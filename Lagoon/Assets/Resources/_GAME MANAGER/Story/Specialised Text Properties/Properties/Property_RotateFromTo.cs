using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class RotateFromTo : Base
        {


            float fromAngle;
            float toAngle;
            float duration;

            TweenManager.TweenPart_Start referenceCurve;

            public RotateFromTo() { }

            // UV = origin of rotation
            public RotateFromTo(float fromAngle_, float toAngle_, float duration_, TweenManager.CURVE_PRESET tween_curve_)
            {
                fromAngle = fromAngle_;
                toAngle = toAngle_;
                duration = duration_;
                referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, tween_curve_);

            }
            public RotateFromTo(float fromAngle_, float toAngle_, float duration_, TweenCurveLibrary curve_library_, string curveID_)
            {
                fromAngle = fromAngle_;
                toAngle = toAngle_;
                duration = duration_;
                referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, curve_library_, curveID_);
            }

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(4, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out fromAngle, lexingArgs, 0))
                    return;
                if (!TryParseFloatWithErrorCheck(out toAngle, lexingArgs, 1))
                    return;
                if (!TryParseFloatWithErrorCheck(out duration, lexingArgs, 2))
                    return;

                if (duration < -0.00001f)
                {
                    lexingArgs.ErrorMessage?.Invoke("Error, parameter cannot be negative", lexingArgs.parameters[2]);
                    return;
                }

                switch (lexingArgs.parameters[3].Data)
                {
                    case "L":
                        {
                            referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, TweenManager.CURVE_PRESET.LINEAR);
                            break;
                        }
                    case "EI":
                        {
                            referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, TweenManager.CURVE_PRESET.EASE_IN);
                            break;
                        }
                    case "EO":
                        {
                            referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, TweenManager.CURVE_PRESET.EASE_OUT);
                            break;
                        }
                    case "EIO":
                        {
                            referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, TweenManager.CURVE_PRESET.EASE_INOUT);
                            break;
                        }
                    case "NEIO":
                        {
                            referenceCurve = new TweenManager.TweenPart_Start(fromAngle, toAngle, duration, TweenManager.CURVE_PRESET.NON_EASE_INOUT);
                            break;
                        }
                    default:
                        {
                            lexingArgs.ErrorMessage?.Invoke("Error, parameter has to be either: L, EI, EO, EIO or NEIO. Look at help node for further info.", lexingArgs.parameters[2]);
                            return;
                        }
                }

            }

            class QuickCharacterTimer
            {
                public float time = 0;
                public int index = 0;
                public float duration = 0;
                public bool IsFinished { get { return (time > duration); } }
                
                public void Update()
                {
                    time += Time.unscaledDeltaTime;
                }
            }
            public override void Init()
            {
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                isHoldingBackFlow = false;
               ;
            }

            // x = list index
            // y = character index 
            List<System.Tuple<int,int>> charactersLeft = new List<System.Tuple<int, int>>();

            List<QuickCharacterTimer> CharacterTimers = new List<QuickCharacterTimer>();
            public override void TransitionStart()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    charactersLeft.Add(new System.Tuple<int, int>(i, specialTextCharacters[i].index));
                }
            }

            public override bool TransitionUpdate(int lowestHoldBackIndex)
            {
                charactersLeft.RemoveAll(y =>
                {
                    if (y.Item2 <= lowestHoldBackIndex)
                    {
                        CharacterTimers.Add(new QuickCharacterTimer() { index = y.Item1, duration = this.duration });
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                );

                bool allTimersFinished = true;
                for (int i = 0; i < CharacterTimers.Count; i++)
                {
                    CharacterTimers[i].Update();
                    specialTextCharacters[CharacterTimers[i].index].rotationDegrees += referenceCurve.GetValue(CharacterTimers[i].time);
                    if (!CharacterTimers[i].IsFinished)
                        allTimersFinished = false;
                }


                if (charactersLeft.Count == 0 && allTimersFinished)
                    return true;
                return false;
            }


            public override void EndlessUpdate()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].rotationDegrees += toAngle;
                }
            }

        }

    }

}