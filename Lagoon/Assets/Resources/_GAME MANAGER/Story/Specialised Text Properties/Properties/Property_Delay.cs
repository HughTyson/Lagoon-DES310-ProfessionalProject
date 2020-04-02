using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialText
{
    public static partial class TextProperties
    {
        public class Delay : NoExitBase
        {
            float duration = 0;
            public Delay()
            {
                
            }
            public Delay(float duration_, int indexOfActivation_)
            {
                duration = duration_;
                indexOfActivation = indexOfActivation_;
            }

            public override void Lex(LexingArgs lexingArgs)
            {

                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out duration, lexingArgs, 0))
                    return;

                if (duration < -0.00001f)
                {
                    lexingArgs.ErrorMessage("Error: parameter cannot be negative.", lexingArgs.parameters[0]);
                    return;
                }
            }


            float current_time = 0;
            public override void Init()
            {
                current_time = duration;
                isHoldingBackFlow = true;
                minCharacterIndex = indexOfActivation;
                maxCharacterIndex = indexOfActivation;
                currentHoldingCharacterIndex = indexOfActivation;
            }
            public override bool TransitionUpdate(int highestHoldBackIndex)
            {
                current_time -= Time.unscaledDeltaTime;

                if (current_time >= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }

}