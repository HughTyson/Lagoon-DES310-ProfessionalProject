using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class Scale : Base
        {
            float size_difference;

            public Scale() { }
            public Scale(float size_difference_)
            {
                size_difference = size_difference_;
            }
            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out size_difference, lexingArgs, 0))
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
                    specialTextCharacters[i].scaleMultiplier *= size_difference;
                }
            }

        }

    }

}