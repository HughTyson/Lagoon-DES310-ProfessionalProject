using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    static class TokenToMarkUpTextFormatter
    {
        static public string DebugTextWithErrorHandlingInTokenization(List<Token> tokenList, TokenError potentialError)
        {
            string output = "";
            bool errorTriggered = false;


            for (int i = 0; i < tokenList.Count; i++)
            {
                if (potentialError.errorFlag)
                {
                    if (tokenList[i] == potentialError.tokenOfError)
                    {
                        errorTriggered = true;
                        output += "<color=#FF0000FF>";
                    }
                }

                switch (tokenList[i].TokenType)
                {
                    case Token.TYPE.TEXT:
                        {
                            output += (errorTriggered) ? "" : "<color=#000000FF>";
                            output += tokenList[i].Data;
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_ENTER:
                        {
                            output += (errorTriggered) ? "" : "<color=#00B050FF>";
                            output += "[";
                            output += tokenList[i].Data;
                            foreach (Token tok in tokenList[i].TokenChildren)
                            {
                                if (tok.TokenType == Token.TYPE.PARAMETER)
                                {
                                    if (potentialError.errorFlag)
                                    {
                                        if (tok == potentialError.tokenOfError)
                                        {
                                            output += "</color>";
                                            errorTriggered = true;
                                            output += "<color=#FF0000FF>";
                                        }
                                    }

                                    if (tok == tokenList[i].TokenChildren[0])
                                        output += "(";

                                    output += (errorTriggered) ? "" : "<color=#0000FFFF>";
                                    output += tok.Data;
                                    output += (errorTriggered) ? "" : "</color>";
                                    if (tok == tokenList[i].TokenChildren[tokenList[i].TokenChildren.Count - 1])
                                    {
                                        if (tok != potentialError.tokenOfError)
                                            output += ")";
                                    }
                                    else
                                    {
                                        output += ",";
                                    }
                                }
                            }

                            output += (potentialError.tokenOfError == tokenList[i]) ? "" : "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_EXIT:
                        {
                            output += (errorTriggered) ? "" : "<color=#ED7D31FF>";
                            output += "[/";
                            output += tokenList[i].Data;
                            output += (potentialError.tokenOfError == tokenList[i]) ? "" : "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_PRESET:
                        {
                            output += (errorTriggered) ? "" : "<color=#00B050FF>";
                            output += "[@";
                            output += tokenList[i].Data;
                            output += (potentialError.tokenOfError == tokenList[i]) ? "" : "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_PRESET_EXIT:
                        {
                            output += (errorTriggered) ? "" : "<color=#ED7D31FF>";
                            output += "[/@";
                            output += tokenList[i].Data;
                            output += (potentialError.tokenOfError == tokenList[i]) ? "" : "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }

                }

            }

            if (potentialError.errorFlag)
            {
                output += System.Environment.NewLine + System.Environment.NewLine +
                        potentialError.errorMessage;
                output += "</color>";
            }
            return output;
        }
        static public string DebugTextWithErrorHandlingInLexing(List<Token> tokenList, TokenError potentialError)
        {
            string output = "";
            bool errorTriggered = false;


            for (int i = 0; i < tokenList.Count; i++)
            {
                if (potentialError.errorFlag)
                {
                    if (tokenList[i] == potentialError.tokenOfError)
                    {
                        errorTriggered = true;
                        output += "<color=#FF0000FF>";
                    }
                }

                switch (tokenList[i].TokenType)
                {
                    case Token.TYPE.TEXT:
                        {
                            output += (errorTriggered) ? "" : "<color=#000000FF>";
                            output += tokenList[i].Data;
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_ENTER:
                        {
                            output += (errorTriggered) ? "" : "<color=#00B050FF>";
                            output += "[";
                            output += tokenList[i].Data;
                            foreach (Token tok in tokenList[i].TokenChildren)
                            {
                                if (tok.TokenType == Token.TYPE.PARAMETER)
                                {
                                    if (potentialError.errorFlag)
                                    {
                                        if (tok == potentialError.tokenOfError)
                                        {
                                            output += "</color>";
                                            errorTriggered = true;
                                            output += "<color=#FF0000FF>";
                                        }
                                    }

                                    if (tok == tokenList[i].TokenChildren[0])
                                        output += "(";

                                    output += (errorTriggered) ? "" : "<color=#0000FFFF>";
                                    output += tok.Data;
                                    output += (errorTriggered) ? "" : "</color>";
                                    if (tok == tokenList[i].TokenChildren[tokenList[i].TokenChildren.Count - 1])
                                    {
                                            output += ")";
                                    }
                                    else
                                    {
                                        output += ",";
                                    }
                                }
                            }

                            output += "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_EXIT:
                        {
                            output += (errorTriggered) ? "" : "<color=#ED7D31FF>";
                            output += "[/";
                            output += tokenList[i].Data;
                            output += "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_PRESET:
                        {
                            output += (errorTriggered) ? "" : "<color=#00B050FF>";
                            output += "[@";
                            output += tokenList[i].Data;
                            output += "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }
                    case Token.TYPE.PROPERTY_PRESET_EXIT:
                        {
                            output += (errorTriggered) ? "" : "<color=#ED7D31FF>";
                            output += "[/@";
                            output += tokenList[i].Data;
                            output += "]";
                            output += (errorTriggered) ? "" : "</color>";
                            break;
                        }

                }

            }

            if (potentialError.errorFlag)
            {
                output += System.Environment.NewLine + System.Environment.NewLine +
                        potentialError.errorMessage;
                output += "</color>";
            }
            return output;
        }
        static public string ParseToDialogText(List<Token> tokenList)
        {
            string output = "";

            for (int i = 0; i < tokenList.Count; i++)
            {
                if (tokenList[i].TokenType == Token.TYPE.TEXT)
                    output += tokenList[i].Data;
            }
            return output;
        }
    }
}