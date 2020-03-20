﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace SpecialText
{
    public class TokenError
    {
        public bool errorFlag = false;
        public string errorMessage = "";
        public Token tokenOfError = null;
        public object additionalInfo = null;
    }

    public static class DebuggingParse
    {
        public static string ParseTextToDebugMarkUpText(string data, GlobalPropertiesNode globalProperties)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> TokenList = tokenizer.Tokenize(data);
            if (tokenizer.GetError().errorFlag)
                return TokenToMarkUpTextFormatter.DebugTextWithErrorHandlingInTokenization(TokenList, tokenizer.GetError());

            Lexer lexer = new Lexer();
            lexer.Lex(TokenList, globalProperties);
           return TokenToMarkUpTextFormatter.DebugTextWithErrorHandlingInLexing(TokenList, lexer.GetError());
           
        }
        public static string ParseTextToDialogOnlyString(string data)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> TokenList = tokenizer.Tokenize(data);
            return TokenToMarkUpTextFormatter.ParseToDialogText(TokenList);
        }
    }

    public class Parser
    {
        Tokenizer tokenizer = new Tokenizer();
        Lexer lexer = new Lexer();
        GlobalPropertiesNode globalProperties;
        public Parser(GlobalPropertiesNode globalProperties_)
        {
            globalProperties = globalProperties_;
        }
        public SpecialTextData ParseToSpecialTextData(string data)
        {
            List<Token> TokenList = tokenizer.Tokenize(data);
            return lexer.Lex(TokenList, globalProperties);
        }
    }


    public class SpecialTextData
    {
    
        public List<TextPropertyData.Base> propertyDataList = new List<TextPropertyData.Base>();
        public List<SpecialTextCharacterData> specialTextCharacters = new List<SpecialTextCharacterData>();
        public string fullTextString = "";
    }
    public class SpecialTextCharacterData
    {
        private readonly static Color32 def_colour = new Color32(0,0,0,0);
        public SpecialTextCharacterData(int index_, char character_)
        {
            index = index_;
            character = character_;
        }
        public readonly int index;
        public readonly char character;
        public void Reset()
        {
            colour = def_colour;
        }
        public Color32 colour = def_colour;
    }

}
