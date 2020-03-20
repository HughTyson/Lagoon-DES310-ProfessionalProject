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
        private readonly static Vector2 def_scaleMultiplier = new Vector2(1, 1);
        private readonly static float def_rotation = 0;
        private readonly static Vector2 def_cetrePposition_offset = new Vector2(0, 0);

        public SpecialTextCharacterData(int index_, char character_)
        {
            index = index_;
            character = character_;
        }
        public void SetupDefVals(Vector2 TopLeft, Vector2 TopRight, Vector2 BottomLeft, Vector2 BottomRight)
        {
            def_centrePos = (TopLeft + TopRight + BottomLeft + BottomRight) / 4.0f;
            def_vertTL = TopLeft - def_centrePos;
            def_vertTR = TopRight - def_centrePos;
            def_vertBL = BottomLeft - def_centrePos;
            def_vertBR = BottomRight - def_centrePos;
        }

        public readonly int index;
        public readonly char character;

        public Vector2 centrePos;
        public Vector2 positionOffset_TopLeft;
        public Vector2 positionOffset_TopRight;
        public Vector2 positionOffset_BottomLeft;
        public Vector2 positionOffset_BottomRight;
        public float rotationDegrees = def_rotation;
        public Color32 colour = def_colour;
        public Vector2 scaleMultiplier = def_scaleMultiplier;
        public Vector2 centrePositionOffset = def_cetrePposition_offset;

        private Vector2 def_centrePos;
        private Vector2 def_vertTL;
        private Vector2 def_vertTR;
        private Vector2 def_vertBL;
        private Vector2 def_vertBR;
        public void Reset()
        {
            colour = def_colour;
            scaleMultiplier = def_scaleMultiplier;
            centrePos = def_centrePos;
            centrePositionOffset = def_cetrePposition_offset;
            positionOffset_TopLeft = def_vertTL;
            positionOffset_TopRight = def_vertTR;
            positionOffset_BottomLeft = def_vertBL;
            positionOffset_BottomRight = def_vertBR;
            rotationDegrees = def_rotation;
        }

        public void ApplyToTMPChar(ref TMPro.TMP_MeshInfo meshInfo, int verticeIndex)
        {
            Color32[] vertexColors = meshInfo.colors32;
            Vector3[] verices = meshInfo.vertices;
            vertexColors[verticeIndex + 0] = colour;
            vertexColors[verticeIndex + 1] = colour;
            vertexColors[verticeIndex + 2] = colour;
            vertexColors[verticeIndex + 3] = colour;
            

            Vector2 topLeft = positionOffset_TopLeft * scaleMultiplier;
            Vector2 topRight = positionOffset_TopRight * scaleMultiplier; 
            Vector2 bottomLeft = positionOffset_BottomLeft * scaleMultiplier; 
            Vector2 bottomRight = positionOffset_BottomRight * scaleMultiplier;

            float height = Mathf.Abs(topLeft.y - bottomLeft.y);
            float width = Mathf.Abs(topLeft.x - topRight.x);
            Vector2 centreOffset = centrePositionOffset * new Vector2(width, height);

            topLeft = Vector2Extension.Rotate(topLeft, rotationDegrees);
            topRight = Vector2Extension.Rotate(topRight, rotationDegrees);
            bottomLeft = Vector2Extension.Rotate(bottomLeft, rotationDegrees);
            bottomRight = Vector2Extension.Rotate(bottomRight, rotationDegrees);

            topLeft += centreOffset;
            topRight += centreOffset;
            bottomLeft += centreOffset;
            bottomRight += centreOffset;
            topLeft += centrePos;
            topRight += centrePos;
            bottomLeft += centrePos;
            bottomRight += centrePos;

            verices[verticeIndex + 0] = topLeft;
            verices[verticeIndex + 1] = topRight;
            verices[verticeIndex + 2] = bottomLeft;
            verices[verticeIndex + 3] = bottomRight;
        }
    }

}
