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
            index = -1;
            depth = 0;
            error.errorFlag = false;

            Itirate();
            while (GetCurrent() != null)
            {
                switch (GetCurrent())
                {
                    case '[':
                        {
                            Token_Property token = new Token_Property(this);
                            TokenList.Add(token);
                            token.Tokenize();
                            break;
                        }
                    default:
                        {
                            Token_Text token = new Token_Text(this);
                            TokenList.Add(token);
                            token.Tokenize();
                            break;
                        }

                }
            }
            return TokenList;
        }

        public void Itirate()
        {
            index++;
            if (index < data.Length)
            {
                current_data = data[index];
            }
            else
            {
                current_data = null;
            }

        }
        public void IterateBack()
        {
            index--;
            if (index < data.Length)
            {
                current_data = data[index];
            }
            else
            {
                current_data = null;
            }
        }

        public char? GetCurrent()
        {
            return current_data;
        }

        public void GoDownTokenLayer()
        {
            depth++;
        }
        public void GoUpTokenLayer()
        {
            depth--;
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
        protected bool errorFlag = false;
        protected string errorMessage = "";

        public bool ErrorFlag { get { return errorFlag; } }
        public string ErrorMessage { get { return errorMessage; } }
        public enum TYPE
        {
            TEXT,
            PROPERTY_ENTER,
            PROPERTY_EXIT,
            PROPERTY_PRESET,
            PROPERTY_PRESET_EXIT,
            PARAMETER
        }
        protected TYPE tokenType;
        protected Tokenizer tokenizer;
        protected string data = "";

        public string Data { get { return data; } }
        public TYPE TokenType { get { return tokenType; } }

        protected List<Token> tokenChildren = new List<Token>();
        public List<Token> TokenChildren { get { return tokenChildren; } }

        public abstract void Tokenize();
    }
    public class Token_Property : Token
    {
        public Token_Property(Tokenizer iterator_)
        {
            tokenType = TYPE.PROPERTY_ENTER;
            tokenizer = iterator_;
        }
        public override void Tokenize()
        {

            while (true)
            {
                tokenizer.Itirate();
                switch (tokenizer.GetCurrent())
                {
                    case null:
                        {
                            tokenizer.FlagError("Error: no ']' found", this);
                            return;
                        }
                    case '[':
                        {
                            tokenizer.FlagError("Error: no ']' found", this);
                            return;
                        }
                    case ']':
                        {
                            tokenizer.Itirate();
                            return;
                        }
                    case '(':
                        {
                            Token_Parameter token = new Token_Parameter(tokenizer);
                            tokenizer.GoDownTokenLayer();
                            tokenizer.AddToken(token);
                            token.Tokenize();
                            tokenizer.GoUpTokenLayer();
                                break;
                        }
                    case '/':
                        {
                            tokenType = TYPE.PROPERTY_EXIT;
                            break;
                        }
                    case '@':
                        {
                            if (tokenType == TYPE.PROPERTY_EXIT)
                            {
                                tokenType = TYPE.PROPERTY_PRESET_EXIT;
                            }
                            else
                            {
                                tokenType = TYPE.PROPERTY_PRESET;
                            }
                            break;
                        }
                    case ' ':
                        {
                            break;
                        }
                    default:
                        {
                            data += tokenizer.GetCurrent();
                            break;
                        }
                }
            }

        }
    }
    public class Token_Parameter : Token
    {
        public Token_Parameter(Tokenizer tokenizer_)
        {
            tokenizer = tokenizer_;
            tokenType = TYPE.PARAMETER;
        }
        public override void Tokenize()
        {
            while (true)
            {
                tokenizer.Itirate();
                switch (tokenizer.GetCurrent())
                {
                    case null:
                        {
                            tokenizer.FlagError("Error: no ')' found", this);
                            return;
                        }
                    case '(':
                        {
                            tokenizer.FlagError("Error: no ')' found", this);
                            return;
                        }
                    case '[':
                        {
                            tokenizer.FlagError("Error: no ')' found", this);
                            tokenizer.IterateBack();
                            return;
                        }
                    case ']':
                        {
                            tokenizer.FlagError("Error: no ')' found", this);
                            tokenizer.IterateBack();
                            return;
                        }
                    case ')':
                        {
                            return;
                        }
                    case ',':
                        {
                            Token_Parameter token = new Token_Parameter(tokenizer);
                            tokenizer.AddToken(token);
                            token.Tokenize();
                            return;
                        }
                    case ' ':
                        {
                            break;
                        }
                    default:
                        {
                            data += tokenizer.GetCurrent();
                            break;
                        }
                }
            }

        }
    }
    public class Token_Text : Token
    {

        public Token_Text(Tokenizer iterator_)
        {
            tokenType = TYPE.TEXT;
            tokenizer = iterator_;
        }

        public override void Tokenize()
        {
            char? current = tokenizer.GetCurrent();
            while (current != null && current != '[')
            {
                data += current;
                tokenizer.Itirate();
                current = tokenizer.GetCurrent();
            }
        }
    }
}