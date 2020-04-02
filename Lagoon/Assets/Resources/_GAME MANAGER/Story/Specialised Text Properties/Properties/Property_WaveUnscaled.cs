using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class WaveUnscaled : Base
        {
            float frequency = 0;
            float amplitude = 0;
            float speed = 0;
            public WaveUnscaled()
            {
            }
            public WaveUnscaled(float frequency_, float amplitude_, float speed_)
            {
                frequency = frequency_;
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

            float time;
            public override void Init()
            {
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                isHoldingBackFlow = false;
            }
            public override void EndlessUpdate()
            {
                time += Time.unscaledDeltaTime;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].centrePositionOffset.y += amplitude * Mathf.Sin((specialTextCharacters[i].index * Mathf.PI * frequency) + (time * speed * Mathf.PI));
                }
            }
        }
    }

}