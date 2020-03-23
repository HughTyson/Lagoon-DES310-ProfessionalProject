using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SpecialText
{
    public static partial class TextProperties
    {
        public abstract class NoExitBase : Base
        {
            protected int indexOfActivation = 0;

            public void SetUpNoExitBase(int indexOfActivation_)
            {
                indexOfActivation = indexOfActivation_;
            }
        }
    }
}
