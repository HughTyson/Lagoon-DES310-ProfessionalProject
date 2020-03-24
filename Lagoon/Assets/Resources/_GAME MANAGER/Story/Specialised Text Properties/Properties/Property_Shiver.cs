using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class Shiver : Base
        {
            float intensity;
            float speed;

            public Shiver() {}
            public Shiver(float intensity_, float speed_)
            {
                intensity = intensity_;
                speed = speed_;
            }

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(2, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out intensity, lexingArgs, 0))
                    return;

                if (!TryParseFloatWithErrorCheck(out speed, lexingArgs, 1))
                    return;
            }

            float x_seed;
            float y_seed;
            float time = 0;

            public override void Init()
            {
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count -1].index;
                isHoldingBackFlow = false;
            }
            public override void TransitionStart()
            {
                x_seed = UnityEngine.Random.Range(0.0f, 100.0f);
                y_seed = UnityEngine.Random.Range(0.0f, 100.0f);
                time = 0;
            }
            public override void EndlessUpdate()
            {
                time += Time.deltaTime;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].centrePositionScaledOffset += new Vector2((Mathf.PerlinNoise(time * speed, x_seed + (float)i) - 0.5f) * 2.0f * intensity, ((Mathf.PerlinNoise(time * speed, y_seed + (float)i) - 0.5f) * intensity * 2.0f));
                }
            }
        }
    }

}