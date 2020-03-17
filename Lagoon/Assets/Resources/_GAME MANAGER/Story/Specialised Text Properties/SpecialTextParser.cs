using System.Collections;
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
        public static string ParseTextToDebugMarkUpText(string data)
        {
            Tokenizer tokenizer = new Tokenizer();
            List<Token> TokenList = tokenizer.Tokenize(data);
            if (tokenizer.GetError().errorFlag)
                return TokenToMarkUpTextFormatter.DebugTextWithErrorHandlingInTokenization(TokenList, tokenizer.GetError());

            Lexer lexer = new Lexer();
            lexer.Lex(TokenList);
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

        public Parser()
        {

        }
        public List<SpecialTextData> ParseToSpecialTextData(string data)
        {
            List<Token> TokenList = tokenizer.Tokenize(data);
            return lexer.Lex(TokenList);
        }
    }


    public class SpecialTextData
    {
        public List<TextPropertyData_Base> propertyDataList = new List<TextPropertyData_Base>();
        public string text;
    }
}
