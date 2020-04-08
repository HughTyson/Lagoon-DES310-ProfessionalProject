using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public class AppearAtOnce : Base
        {
            public static readonly TweenManager.TweenPathBundle transitionIn = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(0.5f, 1, 1, TweenManager.CURVE_PRESET.EASE_OUT)                       // Alpha
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(-45, 0, 1, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")  // Rotation
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(0, 1, 1, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")  // Scale
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(-2.0f, 0, 1, TweenCurveLibrary.DefaultLibrary, "OVERSHOOT")  // Y Position
                    )
                );

            class TransitionVars
            {
                public TypeRef<float> AlphaRef = new TypeRef<float>();
                public TypeRef<float> RotationRef = new TypeRef<float>();
                public TypeRef<float> ScaleRef = new TypeRef<float>();
                public TypeRef<float> YPositionRef = new TypeRef<float>();
            }
            List<TransitionVars> transitionVarsList = new List<TransitionVars>();
            struct TextCharacterWrapper
            {
                public TextCharacterWrapper(SpecialTextCharacterData specialTextCharacterData_, int index)
                {
                    specialTextCharacterData = specialTextCharacterData_;
                    indexInList = index;
                }
                public SpecialTextCharacterData specialTextCharacterData;
                public int indexInList;
            }

            List<TextCharacterWrapper> uncompleteCharacterTransitions = new List<TextCharacterWrapper>();



            static float transition_in_duration = 0.2f;


            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(0, lexingArgs))
                    return;
            }

            int leftToCompleted;
            public override void Init()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    transitionVarsList.Add(new TransitionVars());
                }
                leftToCompleted = specialTextCharacters.Count;
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                currentHoldingCharacterIndex = maxCharacterIndex;
                isHoldingBackFlow = true;
            }


            public override void TransitionStart()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    uncompleteCharacterTransitions.Add(new TextCharacterWrapper(specialTextCharacters[i], i));
                    transitionVarsList[i].AlphaRef.value = 0;
                    transitionVarsList[i].RotationRef.value = 0;
                    transitionVarsList[i].ScaleRef.value = 0;
                    transitionVarsList[i].YPositionRef.value = 0;
                }
            }
            private void characterFinished()
            {
                leftToCompleted--;
            }


            public override bool TransitionUpdate(int lowestHoldBackIndex)
            {
                uncompleteCharacterTransitions.RemoveAll(y =>
                {
                    if (y.specialTextCharacterData.index <= lowestHoldBackIndex)
                    {
                        GM_.Instance.tween_manager.StartTweenInstance
                        (
                            transitionIn,
                            new TypeRef<float>[] {
                            transitionVarsList[y.indexInList].AlphaRef,
                            transitionVarsList[y.indexInList].RotationRef,
                            transitionVarsList[y.indexInList].ScaleRef,
                            transitionVarsList[y.indexInList].YPositionRef
                            },
                            tweenCompleteDelegate_: characterFinished,
                            speed_: 1.0f / transition_in_duration,
                            TimeFormat_: TweenManager.TIME_FORMAT.UNSCALE_DELTA
                        );
                        return true;
                    }
                    return false;
                });

                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = (byte)(255 * transitionVarsList[i].AlphaRef.value);
                    specialTextCharacters[i].scaleMultiplier *= transitionVarsList[i].ScaleRef.value;
                    specialTextCharacters[i].rotationDegrees += transitionVarsList[i].RotationRef.value;
                    specialTextCharacters[i].centrePositionScaledOffset.y += transitionVarsList[i].YPositionRef.value;
                }


                if (leftToCompleted == 0)
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