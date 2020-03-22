using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace SpecialText
{

    public static class TextPropertyData
    {
        public class PropertyInfo
        {
            public PropertyInfo(string name_, Type type_, Type[] soft_incompatibles_ = null , string functionName_ = "", string description_ = "", string parameters_ = "", string example_ = "")
            {
                name = name_;
                type = type_;
                description = description_;
                parameters = parameters_;
                example = example_;
                functionName = functionName_;

                if (soft_incompatibles_ == null)
                    soft_incompatibles = new Type[0];
                else
                    soft_incompatibles = soft_incompatibles_;
            }
            public readonly string name = "";
            public readonly Type type;
            public readonly string functionName = "";
            public readonly string description = "";
            public readonly string parameters = "";
            public readonly string example = "";
            public readonly Type[] soft_incompatibles; // remove all except the closet property causing incompabability
        }

        public static readonly PropertyInfo[] propertyInfos = new PropertyInfo[]
        {
            new PropertyInfo(
                "Colour",
                typeof(Colour),
                functionName_: "Colour(R,G,B)",
                description_: "The RGB colour of the text. Channels are from 0 to 255.",
                parameters_: "R: Red channel. G: Green channel. B: Blue channel.",
                example_: "[Colour(0.5,1,0)] text [/Colour]"
                ),
            new PropertyInfo(
                "Shiver",
                 typeof(Shiver),
                 functionName_: "Shiver(I,S)",
                description_: "Shiver each character of the text.",
                parameters_: "I: Intensity of shiver. S: Speed of shiver.",
                example_: "[Shiver(1, 0.1)] text [/Shiver]"
                ),
            new PropertyInfo(
                "CharSpeed",
                typeof(CharSpeed),
                soft_incompatibles_: new Type[] {typeof(AppearAtOnce)},
                functionName_: "CharSpeed(S)",
                description_: "The speed of showing the characters of the text.",
                parameters_: "S: Speed of showing characters.",
                example_: "[CharSpeed(10)] text [/CharSpeed]"
                ),
            new PropertyInfo(
                "WaveUnscaled",
                typeof(WaveUnscaled),
                functionName_: "Wave(F,A,S)",
                description_: "Moves each character of the text up and down forming a wave",
                parameters_: "F: Frequincy of wave. A: Amplitude of wave. S: Speed of wave",
                example_: "[Wave(3,2,2)] text [/Wave]"
                ),
           new PropertyInfo(
                "WaveScaled",
                typeof(WaveScaled),
                functionName_: "Wave(F,A,S)",
                description_: "Moves each character of the text up and down forming a wave. Scales the frequincy so when there are more or less characters, the wave will be the same",
                parameters_: "F: Frequincy of wave. A: Amplitude of wave. S: Speed of wave",
                example_: "[Wave(3,2,2)] text [/Wave]"
                ),
            new PropertyInfo(
                "Scale",
                typeof(Scale),
                functionName_: "Scale(S)",
                description_: "Scale difference of text in relation to normal text size",
                parameters_: "S: Scale difference of text",
                example_: "[Scale(0.3)] text [/Size]"
                ),
            new PropertyInfo(
                "AppearAtOnce",
                typeof(AppearAtOnce),
                soft_incompatibles_: new Type[] {typeof(CharSpeed) },
                description_: "Shows the text at the same.",
                parameters_: "None",
                example_: "[AppearAtOnce] text [/AppearAtOnce]"
                ),
            new PropertyInfo(
                "StaticAppear",
                typeof(StaticAppear),
                soft_incompatibles_: new Type[] { typeof(CharSpeed), typeof(AppearAtOnce) },
                description_: "Shows the text at the same time, without any transition",
                parameters_: "None",
                example_: "[StaticAppear] text [/StaticAppear]"
                )
        };
        public static readonly PropertyInfo[] noExitPropertyInfo = new PropertyInfo[]
       {
            new PropertyInfo(
                "Delay",
                typeof(Delay),
                functionName_: "Delay(D)",
                description_: "Stop the flow of execution for a period of time",
                parameters_: "D: Delay of execution.",
                example_: "[#Delay(1)]"
                )
       };

        public static readonly Dictionary<int, PropertyInfo> propertyInfoDictionary = propertyInfos.ToDictionary(key => key.name.GetHashCode(), propertyInfo => propertyInfo);
        public static readonly Dictionary<int, PropertyInfo> noExitPropertyInfoDictionary = noExitPropertyInfo.ToDictionary(key => key.name.GetHashCode(), propertyInfo => propertyInfo);
       
        public static readonly Dictionary<Type, PropertyInfo> propertyInfoDictionaryType = propertyInfos.ToDictionary(key => key.type, propertyInfo => propertyInfo);
        public static Base CreatePropertyDataFromName(string name)
        {
            PropertyInfo info;
            if (propertyInfoDictionary.TryGetValue(name.GetHashCode(), out info))
            {
                return (Base)Activator.CreateInstance(info.type);
            }
            return null;
        }
        public static NoExitBase CreateNoExitPropertyDataFromName(string name)
        {
            PropertyInfo info;
            if (noExitPropertyInfoDictionary.TryGetValue(name.GetHashCode(), out info))
            {
                return (NoExitBase)Activator.CreateInstance(info.type);
            }
            return null;
        }

        public static string FindMostSimilarPropertyName(string name)
        {
            string most_similar_name = "";
            int most_similar_value = int.MaxValue;
            for (int i = 0; i < propertyInfos.Length; i++)
            { 
               int new_similarity = StringDistance.GetLevenshteinDistance(name, propertyInfos[i].name);
                if (new_similarity < most_similar_value)
                {
                    most_similar_value = new_similarity;
                    most_similar_name = propertyInfos[i].name;
                }
            }
            return most_similar_name;
        }
        public static string FindMostSimilarNoExitPropertyName(string name)
        {
            string most_similar_name = "";
            int most_similar_value = int.MaxValue;
            for (int i = 0; i < noExitPropertyInfo.Length; i++)
            {
                int new_similarity = StringDistance.GetLevenshteinDistance(name, noExitPropertyInfo[i].name);
                if (new_similarity < most_similar_value)
                {
                    most_similar_value = new_similarity;
                    most_similar_name = noExitPropertyInfo[i].name;
                }
            }
            return most_similar_name;
        }

        public static void ApplyDefaults(SpecialTextData specialTextData, List<Base> included_properties, List<SpecialTextCharacterData> characters ,GlobalPropertiesNode globalProperties )
        {
            if (!included_properties.Exists(y => { return y.GetType() == typeof(AppearAtOnce); }))
            {
                if (!included_properties.Exists(y => { return (y.GetType() == typeof(CharSpeed)); }))
                {
                    Base default_property = new CharSpeed(globalProperties.DefaultSpeedPerTextCharacter, globalProperties.DefaultDurationOfTextCharacterTransitionIn);
                    default_property.AddCharacterReference(characters);
                    specialTextData.propertyDataList.Add(default_property);
                }
            }

        }
        public abstract class Base
        {
            protected GlobalPropertiesNode globalProperties;

            protected List<SpecialTextCharacterData> specialTextCharacters = new List<SpecialTextCharacterData>();

            public PropertyInfo MyInfo
            {
                get { return propertyInfoDictionaryType[this.GetType()]; }
            }


            // Used in the SpecialTextIterator. Where this property is when it comes to stopping the flow of the iterator.
            protected int holdingBackTextFlowIndex;
            protected int highestHoldingTextIndex;
            public int HoldingBackIndex { get { return holdingBackTextFlowIndex; } }

            protected bool IsA_NoCharacterProperty = false;
            // smallerst index value of a character the property affects. 
            protected int lowestCharacterIndex;

            public void SetGlobalPropertiesReference(GlobalPropertiesNode globalProperties_)
            {
                globalProperties = globalProperties_;
            }

            
            public int LowestCharacterIndex { get { return lowestCharacterIndex; } }
            public class LexingArgs
            {
                public readonly Token property;
                public readonly List<Token> parameters;
                public readonly Action<string, Token> ErrorMessage;
                public LexingArgs(Token property_, List<Token> parameters_, Action<string, Token> ErrorMessage_)
                {
                    property = property_;
                    parameters = parameters_;
                    ErrorMessage = ErrorMessage_;
                }   
            }
            

            // LEXING FUNCTIONS
            public abstract void Lex(LexingArgs lexingArgs);
            public void OverrideIncompatibles(List<Base> propertyList, Action<string, Token> ErrorMessage, int myIndex)
            {
                Type[] softIncompatibles = MyInfo.soft_incompatibles;

                for (int i = 0; i < myIndex; i++)
                {
                    for (int k = 0; k < softIncompatibles.Length; k++)
                    {
                        if (propertyList[i].GetType() == softIncompatibles[k].GetType())
                        {
                            propertyList.RemoveAt(i);
                            break;
                        }
                    }
                }


            }
            public void AddCharacterReference(List<SpecialTextCharacterData> textCharacterDatas)
            {
                specialTextCharacters.AddRange(textCharacterDatas);
            }

            public virtual void FinializeLexing(SpecialTextData specialText)
            {
                    if (specialTextCharacters.Count == 0)
                    {
                        specialText.propertyDataList.Remove(this);
                        return;
                    }

                    CompletedLexing();
            }
            public virtual void Init()
            {
                if (specialTextCharacters.Count > 0)
                {
                    lowestCharacterIndex = specialTextCharacters[0].index;
                    highestHoldingTextIndex = specialTextCharacters[specialTextCharacters.Count - 1].index + 1;
                    holdingBackTextFlowIndex = highestHoldingTextIndex; // sets index to highest index, so default is to not hold back the flow of characters.    
                }
            }
            protected virtual void CompletedLexing() { }

            protected static bool ErrorCheckForParameterNum(int desiredParametersNum, LexingArgs lexingArgs)
            {
                if (lexingArgs.parameters.Count != desiredParametersNum)
                {
                    lexingArgs.ErrorMessage?.Invoke("Error: Incorrect number of parameters. Parameter number: " + desiredParametersNum.ToString(), lexingArgs.property);
                    return false;
                }
                for (int i = 0; i < lexingArgs.parameters.Count; i++)
                {
                    if (lexingArgs.parameters[i].Data == "")
                    {
                        lexingArgs.ErrorMessage?.Invoke("Error: Incorrect number of parameters. Parameter number: " + desiredParametersNum.ToString(), lexingArgs.property);
                        return false;
                    }
                }
                return true;
            }
            protected static bool TryParseFloatWithErrorCheck(out float output, LexingArgs lexingArgs, int parameter_index)
            {
                if (!float.TryParse(lexingArgs.parameters[parameter_index].Data, out output))
                {
                    lexingArgs.ErrorMessage?.Invoke("Error: Incorrect parameter type. Parameter must be a float.", lexingArgs.parameters[parameter_index]);
                    return false;
                }
                return true;
            }

            protected static bool TryParseByteWithErrorCheck(out byte output, LexingArgs lexingArgs, int parameter_index)
            {
                int val;
                output = 0;
                if (!int.TryParse(lexingArgs.parameters[parameter_index].Data, out val))
                {
                    lexingArgs.ErrorMessage?.Invoke("Error: Incorrect parameter type. Parameter must be a float.", lexingArgs.parameters[parameter_index]);
                    return false;
                }
                if (val < 0 || val > 255)
                {
                    lexingArgs.ErrorMessage?.Invoke("Error: Parameter must be between 0 and 255.", lexingArgs.parameters[parameter_index]);
                    return false;
                }
                output = (byte)val;
                return true;
            }
            // -- //


            // -- In-Game FUNCTIONS -- //

            public virtual void Begin()
            {

            }
            public virtual bool TransitionUpdate(int highestHoldBackIndex)
            {
                holdingBackTextFlowIndex = specialTextCharacters[specialTextCharacters.Count - 1].index + 1;
                return true;
            }
            public virtual void EndlessUpdate()
            {

            }
            // -- //
        }

        public abstract class NoExitBase : Base
        {
            public override void FinializeLexing(SpecialTextData specialText)
            {
                lowestCharacterIndex = indexOfActivation;
            }
            protected int indexOfActivation = 0;
            public void SetUpNoExitBase(int indexOfActivation_)
            {
                indexOfActivation = indexOfActivation_;
            }
        
        }

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


            public override void EndlessUpdate()
            {
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.r = colour.r;
                    specialTextCharacters[i].colour.g = colour.g;
                    specialTextCharacters[i].colour.b = colour.b;
                }
            }

        }
        public class Shiver : Base
        {
            float intensity;
            float speed;
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
            public override void Begin()
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
                    specialTextCharacters[i].centrePositionScaledOffset += new Vector2((Mathf.PerlinNoise(time * speed, x_seed + (float)i) - 0.5f) * 2.0f * intensity, (( Mathf.PerlinNoise(time * speed, y_seed + (float)i) - 0.5f) * intensity * 2.0f)) ;
                }
            }
        }
       
        public class Delay : NoExitBase
        {
            float duration = 0;
            public Delay()
            {

            }
            public override void Lex(LexingArgs lexingArgs)
            {

                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out duration, lexingArgs, 0))
                    return;

                if (duration < -0.00001f)
                {
                    lexingArgs.ErrorMessage("Error: parameter cannot be negative.", lexingArgs.parameters[0]);
                    return;
                }
            }


            float current_time = 0;
            public override void Begin()
            {
                current_time = duration;
                holdingBackTextFlowIndex = indexOfActivation;
            }
            public override bool TransitionUpdate(int highestHoldBackIndex)
            {
                current_time -= Time.deltaTime;

                if (current_time >= 0)
                {
                    return false;
                }
                else
                {
                    holdingBackTextFlowIndex = int.MaxValue;
                    return true;
                }
            }
        }
        public class WaveUnscaled : Base
        {
            float frequency = 0;
            float amplitude = 0;
            float speed = 0;
            public WaveUnscaled()
            {
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
            public override void Begin()
            {
                time = 0;
            }
            public override void EndlessUpdate()
            {
                time += Time.deltaTime;
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].centrePositionOffset.y += amplitude * Mathf.Sin((specialTextCharacters[i].index * Mathf.PI * frequency) + (time*speed* Mathf.PI));
                }
            }
        }
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
            public override void Begin()
            {
                time = 0;
            }
            public override void EndlessUpdate()
            {
                time += Time.deltaTime;
                holdingBackTextFlowIndex = highestHoldingTextIndex;

                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    float offset = ((float)(specialTextCharacters[i].index - specialTextCharacters[0].index) / (float)(specialTextCharacters[specialTextCharacters.Count - 1].index - specialTextCharacters[0].index));
                    specialTextCharacters[i].centrePositionOffset.y += amplitude * Mathf.Sin((offset * Mathf.PI * frequency) + (time * speed * Mathf.PI));
                }
            }
        }

        public class Scale : Base
        {
            float size_difference;
            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out size_difference, lexingArgs, 0))
                    return;
            }

            public override void EndlessUpdate()
            {
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].scaleMultiplier *= size_difference;
                }
            }

        }
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

            float transition_in_duration;
            int leftToCompleted;

            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(0, lexingArgs))
                    return;

                transition_in_duration = globalProperties.DefaultDurationOfTextCharacterTransitionIn;
            }

            public override void Begin()
            {
                leftToCompleted = specialTextCharacters.Count;
                highestHoldingTextIndex = specialTextCharacters[specialTextCharacters.Count - 1].index;
                skippedFirstTurn = false;
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

            protected override void CompletedLexing()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    transitionVarsList.Add(new TransitionVars());
                }
            }


            bool skippedFirstTurn = false;
            public override bool TransitionUpdate(int lowestHoldBackIndex)
            {

                if (!skippedFirstTurn)
                {
                    skippedFirstTurn = true;
                    return false;
                }
                uncompleteCharacterTransitions.RemoveAll(y =>
                {
                    if (y.specialTextCharacterData.index < lowestHoldBackIndex)
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
                            speed_: 1.0f / transition_in_duration
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
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = 255;
                }
            }

        }
        public class StaticAppear : Base
        {


            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(0, lexingArgs))
                    return;
            }

            public override void Begin()
            {
            }

            protected override void CompletedLexing()
            {
            }

            public override bool TransitionUpdate(int lowestHoldBackIndex)
            {

                bool allComplete = false;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    if (specialTextCharacters[i].index <= lowestHoldBackIndex)
                    {
                        specialTextCharacters[i].colour.a = (byte)(255);
                    }
                    else
                    {
                        allComplete = true;
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
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = 255;
                }
            }


        }

        public class CharSpeed : Base
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
            float speed;
            float transition_in_duration;
            public CharSpeed()
            {

            }
            public CharSpeed(float speed_, float transition_in_duration_)
            {
                speed = speed_;
                transition_in_duration = transition_in_duration_;
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

                transition_in_duration = globalProperties.DefaultDurationOfTextCharacterTransitionIn;

            }
            protected override void CompletedLexing()
            {
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    transitionVarsList.Add(new TransitionVars());
                }
            }

            float time = 0;
            float float_startingIndex;
            int leftToCompleted = 0;
            bool lastCharacterIsTransitioning = false;
            float transitioningFloat = 0;
            public override void Begin()
            {
                time = 0;
                holdingBackTextFlowIndex = specialTextCharacters[0].index;
                float_startingIndex = holdingBackTextFlowIndex;
                leftToCompleted = specialTextCharacters.Count;
                lastCharacterIsTransitioning = false;
                transitioningFloat = float_startingIndex;
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
                    time += Time.deltaTime;
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
                                speed_: 1.0f / transition_in_duration
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

                if (!lastCharacterIsTransitioning)
                {
                    holdingBackTextFlowIndex = Mathf.Min(Mathf.CeilToInt(transitioningFloat), highestHoldingTextIndex);

                    if (holdingBackTextFlowIndex == highestHoldingTextIndex)
                    {
                        lastCharacterIsTransitioning = true;
                    }
                }
                else
                {
                    holdingBackTextFlowIndex = int.MaxValue;
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
                holdingBackTextFlowIndex = highestHoldingTextIndex;
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    specialTextCharacters[i].colour.a = 255;
                }
            }
        }
    }
   
}
