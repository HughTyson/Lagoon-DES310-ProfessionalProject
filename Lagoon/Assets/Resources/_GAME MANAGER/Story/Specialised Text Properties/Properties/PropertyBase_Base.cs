using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpecialText
{
    public static partial class TextProperties
    {
        public abstract class Base
        {

            protected List<SpecialTextCharacterData> specialTextCharacters = new List<SpecialTextCharacterData>();

            public PropertyInfo MyInfo
            {
                get { return propertyInfoDictionaryType[this.GetType()]; }
            }


            protected bool isHoldingBackFlow = false;
            protected int currentHoldingCharacterIndex = 0;

            protected int minCharacterIndex = 0;
            protected int maxCharacterIndex = 0;


            public int HoldingBackIndex { get { return (isHoldingBackFlow) ? currentHoldingCharacterIndex : int.MaxValue; } }


            public int LowestCharacterIndex { get { return minCharacterIndex; } }
            public class LexingArgs
            {
                public readonly Token property;
                public readonly List<Token> parameters;
                public readonly GlobalPropertiesNode globalProperties;
                public readonly Action<string, Token> ErrorMessage;
                public LexingArgs(Token property_, List<Token> parameters_, GlobalPropertiesNode globalProperties_, Action<string, Token> ErrorMessage_)
                {
                    property = property_;
                    parameters = parameters_;
                    ErrorMessage = ErrorMessage_;
                    globalProperties = globalProperties_;
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


            // -- In-Game FUNCTIONS -- //
            public virtual void Init() { }

            public virtual void TransitionStart() { }

            public virtual bool TransitionUpdate(int lowestHoldBackIndex)
            {
                EndlessUpdate();
                return true;
            }
            public virtual void EndlessUpdate() { }

            // -- //

            // -- STATIC -- //
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
        }
    }
}