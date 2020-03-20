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
            public PropertyInfo(string name_, Type type_,string functionName_ = "", string description_ = "", string parameters_ = "", string example_ = "")
            {
                name = name_;
                type = type_;
                description = description_;
                parameters = parameters_;
                example = example_;
                functionName = functionName_;
            }
            public readonly string name = "";
            public readonly Type type;
            public readonly string functionName = "";
            public readonly string description = "";
            public readonly string parameters = "";
            public readonly string example = "";
        }

        static readonly PropertyInfo[] propertyInfos = new PropertyInfo[]
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
                functionName_: "CharSpeed(S)",
                description_: "The speed of showing the characters of the text.",
                parameters_: "S: Speed of showing characters.",
                example_: "[CharSpeed(0.3)] text [/CharSpeed]"
                ),
            new PropertyInfo(
                "Wave",
                typeof(Wave),
                functionName_: "Wave(F,A,S)",
                description_: "Moves each character of the text up and down forming a wave",
                parameters_: "F: Frequincy of wave. A: Amplitude of wave. S: Speed of wave",
                example_: "[Wave(3,2,2)] text [/Wave]"
                ),
            new PropertyInfo(
                "Size",
                typeof(Size),
                functionName_: "Size(S)",
                description_: "Size difference of text in relation to normal text size",
                parameters_: "S: Size difference of text",
                example_: "[Size(0.3)] text [/Size]"
                ),
            new PropertyInfo(
                "AppearFromAFar",
                typeof(AppearFromAFar),
                description_: "",
                parameters_: "",
                example_: "[AppearFromAFar()] text [/AppearFromAFar]"
                )
        };

        public static readonly Dictionary<int, PropertyInfo> propertyInfoDictionary = propertyInfos.ToDictionary(key => key.name.GetHashCode(), propertyInfo => propertyInfo);
        public static Base CreatePropertyDataFromName(string name)
        {
            PropertyInfo info;
            if (propertyInfoDictionary.TryGetValue(name.GetHashCode(), out info))
            {
                return (Base)Activator.CreateInstance(info.type);
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

        public static void ApplyDefaults(SpecialTextData specialTextData, List<Base> included_properties, List<SpecialTextCharacterData> characters ,GlobalPropertiesNode globalProperties )
        {
            if (!included_properties.Exists(y => { return y.GetType() == typeof(AppearFromAFar); }))
            {
                if (!included_properties.Exists(y => { return (y.GetType() == typeof(CharSpeed)); }))
                {
                    Base default_property = new CharSpeed(globalProperties.DefaultSpeedPerTextCharacter);
                    default_property.AddCharacterReference(characters);
                    specialTextData.propertyDataList.Add(default_property);
                }
            }

        }
        public abstract class Base
        {

            protected List<SpecialTextCharacterData> specialTextCharacters = new List<SpecialTextCharacterData>();

            

            // Used in the SpecialTextIterator. Where this property is when it comes to stopping the flow of the iterator.
            protected int holdingBackTextFlowIndex;
            protected int highestHoldingTextIndex;
            public int HoldingBackIndex { get { return holdingBackTextFlowIndex; } }

            // smallerst index value of a character the property affects. 
            int lowestCharacterIndex;

            
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
            public virtual void CheckForIncompatibles(List<Base> propertyList, Action<string, Token> ErrorMessage)
            {
                propertyList.Remove(this);
            }
            public void AddCharacterReference(List<SpecialTextCharacterData> textCharacterDatas)
            {
                specialTextCharacters.AddRange(textCharacterDatas);
            }
            public void FinializeLexing(SpecialTextData specialText)
            {
                if (specialTextCharacters.Count == 0)
                {
                    specialText.propertyDataList.Remove(this);
                    return;
                }

                lowestCharacterIndex = specialTextCharacters[0].index;
                highestHoldingTextIndex = specialTextCharacters[specialTextCharacters.Count - 1].index + 1;
                holdingBackTextFlowIndex = highestHoldingTextIndex; // sets index to highest index, so default is to not hold back the flow of characters.
            }
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
            public virtual bool TransitionUpdate()
            {
                holdingBackTextFlowIndex = specialTextCharacters[specialTextCharacters.Count - 1].index + 1;
                return true;
            }
            public virtual void EndlessUpdate()
            {

            }
            // -- //





        }
        public class Colour : Base
        {
            Color32 colour = new Color(0, 0, 0, 255);
            public Colour()
            {
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

            float time = 0;
            float float_startingIndex;
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

                if (!TryParseFloatWithErrorCheck(out speed, lexingArgs, 0))
                    return;
            }
        }
        public class CharSpeed : Base
        {
            float speed;
            
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
            public override void Begin()
            {
                time = 0;
                holdingBackTextFlowIndex = specialTextCharacters[0].index ;
                float_startingIndex = holdingBackTextFlowIndex;
            }
            public override bool TransitionUpdate()
            {
                time += Time.deltaTime;

                if (time > (specialTextCharacters.Count / speed))
                {
                    EndlessUpdate();
                    return true;
                }
                float transitioningFloat = float_startingIndex + (time * speed);
                for (int i = 0; i < specialTextCharacters.Count; i++)
                {
                    if (specialTextCharacters[i].index < transitioningFloat)
                    {
                        specialTextCharacters[i].colour.a = 255;
                    }
                    else
                    {
                        specialTextCharacters[i].colour.a = 0;
                    }
                }
                holdingBackTextFlowIndex = (int)Mathf.Min(transitioningFloat, highestHoldingTextIndex);
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
        //public class Delay : Base
        //{
        //    float duration = 0;
        //    public Delay()
        //    {
        //    }
        //    public override void Lex(LexingArgs lexingArgs)
        //    {

        //        if (!ErrorCheckForParameterNum(1, lexingArgs))
        //            return;

        //        if (!TryParseFloatWithErrorCheck(out duration, lexingArgs, 0))
        //            return;

        //        if (duration < -0.00001f)
        //        {
        //            lexingArgs.ErrorMessage("Error: parameter cannot be negative.", lexingArgs.parameters[0]);
        //            return;
        //        }
        //    }
        //}
        public class Wave : Base
        {
            float frequency = 0;
            float amplitude = 0;
            float speed = 0;
            public Wave()
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
        }
        public class Size : Base
        {
            float size_difference;
            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(1, lexingArgs))
                    return;

                if (!TryParseFloatWithErrorCheck(out size_difference, lexingArgs, 0))
                    return;
            }

        }
        public class AppearFromAFar : Base
        {
            public override void Lex(LexingArgs lexingArgs)
            {
                if (!ErrorCheckForParameterNum(0, lexingArgs))
                    return;
            }
        }
    }
   
}
