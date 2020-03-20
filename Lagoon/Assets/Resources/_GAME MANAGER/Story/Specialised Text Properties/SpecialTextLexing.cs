﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    class Lexer
    {
        List<SpecialTextData> specialTextData = new List<SpecialTextData>();
        TokenError error = new TokenError();
        public Lexer()
        {
        }


        public TokenError GetError()
        {
            return error;
        }
        public void FlagError(string message, Token tokenOfError)
        {
            if (!error.errorFlag)
            {
                error.errorFlag = true;
                error.errorMessage = message;
                error.tokenOfError = tokenOfError;
            }
        }

        struct OpenProperty
        {
            public OpenProperty(TextPropertyData.Base property, Token token)
            {
                property_data_version = property;
                token_version = token;
            }
            public TextPropertyData.Base property_data_version;
            public Token token_version;
        }

        public SpecialTextData Lex(List<Token> tokenList, GlobalPropertiesNode globalProperties)
        {
            SpecialTextData specialTextData = new SpecialTextData();
            if (tokenList.Count > 0)
            {
                if (globalProperties == null)
                {
                    FlagError("Error: Global Properties Node doesn't exist.", tokenList[0]);
                }
            }



            List<OpenProperty> openProperties = new List<OpenProperty>();

            for (int i = 0; i < tokenList.Count; i++)
            {
                if (error.errorFlag)
                {
                    return null;
                }

                switch (tokenList[i].TokenType)
                {
                    case Token.TYPE.PROPERTY_ENTER:
                        {

                            TextPropertyData.Base property_data = TextPropertyData.CreatePropertyDataFromName(tokenList[i].Data);
                            if (property_data != null)
                            {
                                property_data.SetGlobalPropertiesReference(globalProperties);
                                specialTextData.propertyDataList.Add(property_data);
                                property_data.Lex(new TextPropertyData.Base.LexingArgs(tokenList[i], tokenList[i].TokenChildren, FlagError));
                                openProperties.Add(new OpenProperty(property_data, tokenList[i]));
                            }
                            else
                            {
                                FlagError("Error: Property Type Not Found. Did you mean: " + TextPropertyData.FindMostSimilarPropertyName(tokenList[i].Data) + "?", tokenList[i]);
                            }
                            break;
                        }
                    case Token.TYPE.PROPERTY_EXIT:
                        {
                            TextPropertyData.PropertyInfo property_info;
                            
                            if (TextPropertyData.propertyInfoDictionary.TryGetValue(tokenList[i].Data.GetHashCode(), out property_info))
                            {
                                bool found = false;
                                for (int k = openProperties.Count - 1; k >= 0; k--) // goes backwards so if multiple same properties are opened, remove the inner most one
                                {
                                    if (openProperties[k].property_data_version.GetType() == property_info.type)
                                    {
                                        openProperties.RemoveAt(k);
                                        found = true;
                                        break;
                                    }
                                }
                                if (!found)
                                {
                                    FlagError("Error: Property closed but never opened.", tokenList[i]);
                                    break;
                                }
                            }
                            else
                            {
                                FlagError("Error: Property Type Not Found. Did you mean: " + TextPropertyData.FindMostSimilarPropertyName(tokenList[i].Data) + "?", tokenList[i]);
                                break;
                            }
                            break;
                        }
                    case Token.TYPE.TEXT:
                        {
             
                            specialTextData.fullTextString += ((Token_Text)tokenList[i]).Data;
      
                            List<TextPropertyData.Base> appliedProperties = new List<TextPropertyData.Base>();

                            for (int k = openProperties.Count - 1; k >= 0; k--)
                            {
                                // prevent property duplicates. Only use most recently opened property, which is the inner most duplicate. 
                                if (!appliedProperties.Exists(y => { return (y.GetType() == openProperties[k].property_data_version.GetType()); }))
                                {
                                    appliedProperties.Add(openProperties[k].property_data_version);
                                }
                            }

                            List<TextPropertyData.Base> incompatibleCheck = new List<TextPropertyData.Base>(appliedProperties);
                            for (int k = incompatibleCheck.Count - 1; k >= 0; k--) // start from back to allow removing of indexes inside the loop
                            {
                                incompatibleCheck[k].CheckForIncompatibles(incompatibleCheck, FlagError);
                            }

                            List<SpecialTextCharacterData> textCharacters = new List<SpecialTextCharacterData>();

                            for (int k = 0; k < ((Token_Text)tokenList[i]).Data.Length; k++)
                            {
                                textCharacters.Add(new SpecialTextCharacterData(specialTextData.specialTextCharacters.Count + textCharacters.Count, ((Token_Text)tokenList[i]).Data[k]));
                            }
                            for (int k = 0; k < appliedProperties.Count; k++)
                            {
                                appliedProperties[k].AddCharacterReference(textCharacters);
                            }
                            TextPropertyData.ApplyDefaults(specialTextData, appliedProperties, textCharacters, globalProperties);

                            specialTextData.specialTextCharacters.AddRange(textCharacters);
                            break;
                        }
                    default:
                        {
                            FlagError("Error: Unkown error in this area.", tokenList[i]);
                            break;
                        }
                }

            }

            if (openProperties.Count != 0)
            {
                FlagError("Error: Property opened but never closed.", openProperties[0].token_version);
            }



            for (int i = 0; i < specialTextData.propertyDataList.Count; i++)
            {
                specialTextData.propertyDataList[i].FinializeLexing(specialTextData);
            }

            return specialTextData;
        }
    }
}