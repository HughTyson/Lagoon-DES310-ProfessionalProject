using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SpecialText
{
    public class Tokenizer
    {
        string data;
        int index;
        int depth;
        List<Token> TokenList = new List<Token>();
        char? current_data = null;

        TokenError error = new TokenError();


        public Tokenizer()
        {
        }
        public List<Token> Tokenize(string data_)
        {
            TokenList.Clear();
            data = data_;
            error.errorFlag = false;


            int iterator = 0;

            if (data_ == null)
                return new List<Token>();

            while (iterator < data.Length)
            {

                // Property Token
                if (data_[iterator] == '[')
                {
                    string property_data = "";
                    iterator++;
                    bool found = false;

                    while(iterator < data.Length && !found)
                    {
                        if (data_[iterator] == ']')
                        {
                            found = true;
                        }
                        else
                        {
                            property_data += data_[iterator];
                        }
                        iterator++;
                    }
                    property_data = property_data.Trim(' ');
                    Token_Property propertyToken = new Token_Property(this, property_data);
                    TokenList.Add(propertyToken);
                    if (!found)
                    {
                        FlagError("Error. No ']' found.", propertyToken);
                    }
                }
                // Text Token
                else
                {
                    string text_data = "";
                    while (iterator < data.Length)
                    {
                        if (data_[iterator] == '[')
                        {
                            break;
                        }
                        else
                        {
                            text_data += data_[iterator];
                        }
                        iterator++;
                    }
                    Token_Text textToken = new Token_Text(this, text_data);
                    TokenList.Add(textToken);
                }
            }
            return TokenList;
        }
        public void AddToken(Token token)
        {
            List<Token> tokenLayer = TokenList;

            for (int i = 0; i < depth; i++)
            {
                tokenLayer = tokenLayer[tokenLayer.Count - 1].TokenChildren;
            }
            tokenLayer.Add(token);
        }

        public void FlagError(string error_message, Token tokenOfError)
        {
            if (!error.errorFlag)
            {
                error.errorMessage = error_message;
                error.errorFlag = true;
                error.tokenOfError = tokenOfError;
            }

        }
        public TokenError GetError()
        {
            return error;

        }
    }

    public abstract class Token
    {
        public enum TYPE
        {
            TEXT,
            PROPERTY_ENTER,
            PROPERTY_EXIT,
            PROPERTY_PRESET,
            PROPERTY_PRESET_EXIT,
            PROPERTY_NOEXIT,
            PARAMETER
        }
        protected TYPE tokenType;
        protected Tokenizer tokenizer;
        protected string data = "";

        public string Data { get { return data; } }
        public TYPE TokenType { get { return tokenType; } }

        protected List<Token> tokenChildren = new List<Token>();
        public List<Token> TokenChildren { get { return tokenChildren; } }
    }
    public class Token_Property : Token
    {
        public Token_Property(Tokenizer iterator_, string data_)
        {

            tokenizer = iterator_;

            data = "";
            int iterator = 0;
            


            // Search for Parameter Tokens
            while (iterator < data_.Length)
            {
                if (data_[iterator] == '(')
                {
                    string parameter_data = "";
                    iterator++;
                    bool found = false;

                    while (iterator < data_.Length && !found)
                    {
                        if (data_[iterator] == ')')
                        {
                            found = true;
                        }
                        else if (data_[iterator] == ',')
                        {
                            tokenChildren.Add(new Token_Parameter(tokenizer, parameter_data));
                            parameter_data = "";
                        }
                        else
                        {
                            parameter_data += data_[iterator];
                        }
                        iterator++;
                    }
                    tokenChildren.Add(new Token_Parameter(tokenizer, parameter_data));
                    if (!found)
                    {
                        tokenizer.FlagError("Error. No ')' found.", this);
                    }
                    else
                    {
                        if (iterator < data_.Length)
                        {
                            if (data_[iterator] != ']')
                            {
                                tokenizer.FlagError("Error. Unknown characters inside properties after parameters", this);
                            }
                        }
                    }
                }
                else
                {
                    data += data_[iterator];
                    iterator++;
                }
            }

            if (data.Length > 0)
            {
                switch (data[0])
                {
                    case '/':
                        {
                            if (data.Length > 1)
                            {
                                if (data[1] == '@')
                                {
                                    tokenType = TYPE.PROPERTY_PRESET_EXIT;
                                    data = data.Remove(0, 2);
                                    break;
                                }
                            }
                            tokenType = TYPE.PROPERTY_EXIT;
                            data = data.Remove(0, 1);
                            break;
                        }
                    case '@':
                        {
                            tokenType = TYPE.PROPERTY_PRESET;
                            data = data.Remove(0, 1);
                            break;
                        }
                    case '#':
                        {
                            tokenType = TYPE.PROPERTY_NOEXIT;
                            data = data.Remove(0, 1);
                            break;
                        }
                    default: tokenType = TYPE.PROPERTY_ENTER; break;
                }
            }


            if (data.Length == 0)
            {
                tokenizer.FlagError("Error, no property in brackets", this);
            }

            // Error Check for illegal case
            for (int i = 0; i < data.Length; i++)
            {
               if (data[i] == '#' || data[i] == '@' || data[i] == '/')
                {
                    if (!(i == 0 && data[i] == '@' && tokenType == TYPE.PROPERTY_PRESET_EXIT)) // exception
                    {
                        tokenizer.FlagError("Error, illegal characters in property", this);
                    }                   
                }
            }
        }


    }
    public class Token_Parameter : Token
    {
        public Token_Parameter(Tokenizer tokenizer_, string data_)
        {
            data = data_;
            tokenizer = tokenizer_;
            tokenType = TYPE.PARAMETER;
        }
    }
    public class Token_Text : Token
    {

        public Token_Text(Tokenizer iterator_, string data_)
        {
            tokenType = TYPE.TEXT;
            tokenizer = iterator_;
            data = data_;
        }
    }
}