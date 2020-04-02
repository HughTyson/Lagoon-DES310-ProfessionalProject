using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialText
{
    public static partial class TextProperties
    {
        public class CharSpeed : Base
        {


            public static readonly TweenManager.TweenPathBundle transitionIn = new TweenManager.TweenPathBundle(
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(0.5f, 1, 1, TweenManager.CURVE_PRESET.EASE_OUT)                       // Alpha
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(-45, 0, 1, TweenManager.CURVE_PRESET.EASE_OUT)  // Rotation
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(0, 1, 1, TweenManager.CURVE_PRESET.EASE_OUT)  // Scale
                    ),
                new TweenManager.TweenPath(
                    new TweenManager.TweenPart_Start(-2.0f, 0, 1, TweenManager.CURVE_PRESET.EASE_OUT)  // Y Position
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
            float speed;

            static float transition_in_duration = 0.2f;
            public CharSpeed()
            {

            }
            public CharSpeed(float speed_)
            {
                speed = speed_;
            }

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out speed, lexingArgs, 0))
                    return;

                if (speed < -0.00001f)
                {
                    lexingArgs.ErrorMessage("Error: parameter cannot be negative.", lexingArgs.parameters[0]);
                    return;
                }
            }


            float time = 0;
            float float_startingIndex;
            int leftToCompleted = 0;
            float transitioningFloat = 0;


            public override void Init()
            {
                isHoldingBackFlow = true;
                minCharacterIndex = specialTextCharacters[0].index;
                maxCharacterIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                currentHoldingCharacterIndex = minCharacterIndex;
                float_startingIndex = minCharacterIndex;

                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    transitionVarsList.Add(new TransitionVars());
                }
                time = 0;

                leftToCompleted = specialTextCharacters.Count;
                transitioningFloat = minCharacterIndex;
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

                if (float_startingIndex + (time * speed) <= lowestHoldBackIndex)
                {
                    time += Time.unscaledDeltaTime;
                    transitioningFloat = float_startingIndex + (time * speed);

                    uncompleteCharacterTransitions.RemoveAll(y =>
                    {
                        if (y.specialTextCharacterData.index < transitioningFloat)
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
                }
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = (byte)(255 * transitionVarsList[i].AlphaRef.value);
                    specialTextCharacters[i].scaleMultiplier *= transitionVarsList[i].ScaleRef.value;
                    specialTextCharacters[i].rotationDegrees += transitionVarsList[i].RotationRef.value;
                    specialTextCharacters[i].centrePositionScaledOffset.y += transitionVarsList[i].YPositionRef.value;
                }

                currentHoldingCharacterIndex = Mathf.CeilToInt(transitioningFloat);
                if (currentHoldingCharacterIndex > maxCharacterIndex)
                {
                    isHoldingBackFlow = false;
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
