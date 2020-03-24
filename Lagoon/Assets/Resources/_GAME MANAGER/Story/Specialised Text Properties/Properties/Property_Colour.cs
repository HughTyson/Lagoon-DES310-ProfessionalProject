using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialText
{
    public static partial class TextProperties
    {
        public class Colour : Base
        {
            Color32 colour = new Color(0, 0, 0, 255);
            public Colour()
            {
            }
            public Colour(byte R, byte G, byte B)
            {
                colour.r = R;
                colour.g = G;
                colour.b = B;
            }

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(3, lexingArgs))
                    return;


                byte[] vals = new byte[lexingArgs.parameters.Count];
                for (int i = 0; i < lexingArgs.parameters.Count; i++)
                {
                    if (!TryParseByteWithErrorCheck(out vals[i], lexingArgs, i))
                        return;
                }
                colour.r = vals[0];
                colour.g = vals[1];
                colour.b = vals[2];
            }


            public override void Init()
            {
                isHoldingBackFlow = false;
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
            }
            public override void EndlessUpdate()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.r = colour.r;
                    specialTextCharacters[i].colour.g = colour.g;
                    specialTextCharacters[i].colour.b = colour.b;
                }
            }

        }

    }

}