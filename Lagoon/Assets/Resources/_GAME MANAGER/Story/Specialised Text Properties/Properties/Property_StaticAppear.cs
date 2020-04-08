using System.Collections;
using System.Collections.Generic;
using UnityEngine;




namespace SpecialText
{
    public static partial class TextProperties
    {
        public class StaticAppear : Base
        {
            public StaticAppear()
            {

            }

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(0, lexingArgs))
                    return;
            }

            public override void Init()
            {
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                isHoldingBackFlow = false;
            }


            public override bool TransitionUpdate(int lowestHoldBackIndex)
            {

                bool allComplete = true;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    if (specialTextCharacters[i].index <= lowestHoldBackIndex)
                    {
                        specialTextCharacters[i].colour.a = (byte)(255);
                    }
                    else
                    {
                        allComplete = false;
                        break;
                    }
                }

                if (allComplete)
                {
                    EndlessUpdate();
                    return true;
                }
                return false;
            }

            public override void EndlessUpdate()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = 255;
                }
            }


        }
    }
}