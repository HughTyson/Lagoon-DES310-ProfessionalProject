using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class Rotate : Base
        {
            float angle;

            public Rotate() { }

            // UV = origin of rotation
            public Rotate(float angle_)
            {
                angle = angle_;
            }
            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out angle, lexingArgs, 0))
                    return;
            }

            public override void Init()
            {
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                isHoldingBackFlow = false;
            }
            public override void EndlessUpdate()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].rotationDegrees += angle;
                }
            }

        }

    }

}