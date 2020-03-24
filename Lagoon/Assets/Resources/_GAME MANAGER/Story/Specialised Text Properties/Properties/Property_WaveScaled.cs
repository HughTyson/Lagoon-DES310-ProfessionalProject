using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
            public class WaveScaled : Base
            {
                float frequency = 0;
                float amplitude = 0;
                float speed = 0;
                public WaveScaled()
                {
                }
                public WaveScaled(float frequincy_, float amplitude_, float speed_)
                {
                    frequency = frequincy_;
                    amplitude = amplitude_;
                    speed = speed_;
                }

                public override void Lex(LexingArgs lexingArgs)
                {
                    if (!ErrorCheckForParameterNum(3, lexingArgs))
                        return;

                    if (!TryParseFloatWithErrorCheck(out frequency, lexingArgs, 0))
                        return;
                    if (!TryParseFloatWithErrorCheck(out amplitude, lexingArgs, 1))
                        return;
                    if (!TryParseFloatWithErrorCheck(out speed, lexingArgs, 2))
                        return;
                }

                float time = 0;

            public override void Init()
            {
                isHoldingBackFlow = false;
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
            }
            public override void TransitionStart()
                {
                    time = 0;
                }
                public override void EndlessUpdate()
                {
                    time += Time.deltaTime;

                    for (int i = 0; i < specialTextCharacters.Count; i++)
                    {
                        float offset = ((float)(specialTextCharacters[i].index - specialTextCharacters[0].index) / (float)(specialTextCharacters[specialTextCharacters.Count - 1].index - specialTextCharacters[0].index));
                        specialTextCharacters[i].centrePositionOffset.y += amplitude * Mathf.Sin((offset * Mathf.PI * frequency) + (time * speed * Mathf.PI));
                    }
                }
            }
    }
}