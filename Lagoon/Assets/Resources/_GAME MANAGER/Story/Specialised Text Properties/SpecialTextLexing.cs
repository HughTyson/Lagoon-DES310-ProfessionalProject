using System.Collections;
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
            public OpenProperty(TextPropertyData_Base property, Token token)
            {
                property_data_version = property;
                token_version = token;
            }
            public TextPropertyData_Base property_data_version;
            public Token token_version;
        }

        public List<SpecialTextData> Lex(List<Token> tokenList)
        {

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
                            switch (GetEnumTypeFromToken((Token_Property)tokenList[i]))
                            {
                                case TextPropertyData_Base.TYPE.COLOUR:
                                    {
                                        TextPropertyData_Colour property_data = new TextPropertyData_Colour();
                                        property_data.Lex(tokenList[i], tokenList[i].TokenChildren, FlagError);
                                        openProperties.Add(new OpenProperty(property_data, tokenList[i]));
                                        break;
                                    }
                                case TextPropertyData_Base.TYPE.SHIVER:
                                    {
                                        TextPropertyData_Shiver property_data = new TextPropertyData_Shiver();
                                        property_data.Lex(tokenList[i], tokenList[i].TokenChildren, FlagError);
                                        openProperties.Add(new OpenProperty(property_data, tokenList[i]));
                                        break;
                                    }
                                default:
                                    {
                                        FlagError("Error: Property Type Not Found. Did you mean: " + MostSimilarPropertyName(tokenList[i].Data) + "?", tokenList[i]);
                                        break;
                                    }
                            }
                            break;
                        }
                    case Token.TYPE.PROPERTY_EXIT:
                        {
                            TextPropertyData_Base.TYPE? property_type = GetEnumTypeFromToken((Token_Property)tokenList[i]);

                            if (property_type == null)
                            {
                                FlagError("Error: Property Type Not Found. Did you mean: " + MostSimilarPropertyName(tokenList[i].Data) + "?", tokenList[i]);
                                break;
                            }
                            else
                            {
                                bool found = false;
                                for (int k = openProperties.Count - 1; k >= 0; k--) // goes backwards so if multiple same properties are opened, remove the inner most one
                                {

                                    if (openProperties[k].property_data_version.DataType == property_type)
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
                            break;
                        }
                    case Token.TYPE.TEXT:
                        {
                            SpecialTextData newSpecialTextData = new SpecialTextData();
                            newSpecialTextData.text = ((Token_Text)tokenList[i]).Data;
                            for (int k = 0; k < openProperties.Count; k++)
                            {
                                newSpecialTextData.propertyDataList.Add(openProperties[k].property_data_version);
                            }
                            specialTextData.Add(newSpecialTextData);
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

            return specialTextData;
        }

        TextPropertyData_Base.TYPE? GetEnumTypeFromToken(Token_Property token)
        {
            switch (token.Data)
            {
                case "colour": { return TextPropertyData_Base.TYPE.COLOUR; }
                case "shiver": { return TextPropertyData_Base.TYPE.SHIVER; }
                default: return null;
            }
        }
        string MostSimilarPropertyName(string name)
        {
            string property_name = "colour";
            string most_similar = property_name;
            int most_similar_similarity = StringDistance.GetDamerauLevenshteinDistance(name, property_name);

            property_name = "shiver";
            int similarity = StringDistance.GetDamerauLevenshteinDistance(name, property_name);
            if (similarity < most_similar_similarity)
            {
                most_similar_similarity = similarity;
                most_similar = property_name;
            }

            return most_similar;
        }
    }
}