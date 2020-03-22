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
        public void FillTextWithProperties(List<TextPropertyData.Base> properties, int start_index, int end_index)
        {
            propertyDataList.AddRange(properties);
            for (int i = 0; i < properties.Count; i++)
            {
                properties[i].AddCharacterReference(specialTextCharacters.GetRange(start_index, start_index + (end_index - start_index)));
            }

        }
        public void CreateCharacterData(string text)
        {
            fullTextString = text;
            for (int i = 0; i < text.Length; i++)
                specialTextCharacters.Add(new SpecialTextCharacterData(i, text[i]));
        }
        public SpecialTextData() { }

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


        struct InitVals
        {
            public Vector2 Bottom_Left;
            public Vector2 Top_Left;
            public Vector2 Top_Right;
            public Vector2 Bottom_Right;
            public Vector2 Centre;
            public Color32 Colour;
        }
        InitVals initVals;
        public SpecialTextCharacterData(int index_, char character_)
        {
            index = index_;
            character = character_;
        }


        public void SetupInitialVals(Vector2 Bottom_Left_, Vector2 Top_Left_, Vector2 Top_Right_, Vector2 Bottom_Right_, Color32 colour_)
        {
            Vector2 centre = (Bottom_Left_ + Top_Left_ + Top_Right_ + Bottom_Right_) / 4.0f;
            initVals = new InitVals {
                Bottom_Left = Bottom_Left_ - centre,
                Top_Left = Top_Left_ - centre,
                Top_Right = Top_Right_ - centre,
                Bottom_Right = Bottom_Right_ - centre,
                Colour = colour_ , 
                Centre = centre
            };
        }
        public void SetupDefVals(Vector2 Bottom_Left, Vector2 Top_Left, Vector2 Top_Right, Vector2 Bottom_Right)
        {
            def_centrePos = (Bottom_Left + Top_Left + Top_Right + Bottom_Right) / 4.0f;
            def_vert_BL = Bottom_Left - def_centrePos;
            def_vert_TL = Top_Left - def_centrePos;
            def_vert_TR = Top_Right - def_centrePos;
            def_vert_BR = Bottom_Right - def_centrePos;
        }

        public readonly int index;
        public readonly char character;

        public Vector2 centrePositionOffset;
        public Vector2 positionOffset_BottomLeft;
        public Vector2 positionOffset_TopLeft;
        public Vector2 positionOffset_TopRight;
        public Vector2 positionOffset_BottomRight;
        public float rotationDegrees = def_rotation;
        public Color32 colour = def_colour;
        public Vector2 scaleMultiplier = def_scaleMultiplier;
        public Vector2 centrePositionScaledOffset = def_cetrePposition_offset; // scaled offset based on the characters width and height.

        private Vector2 def_centrePos;
        private Vector2 def_vert_BL;
        private Vector2 def_vert_TL;
        private Vector2 def_vert_TR;
        private Vector2 def_vert_BR;
        public void Reset()
        {
            colour = def_colour;
            scaleMultiplier = def_scaleMultiplier;
            centrePositionOffset = def_centrePos;
            centrePositionScaledOffset = def_cetrePposition_offset;
            positionOffset_BottomLeft = def_vert_BL;
            positionOffset_TopLeft = def_vert_TL;
            positionOffset_TopRight = def_vert_TR;
            positionOffset_BottomRight = def_vert_BR;
            rotationDegrees = def_rotation;
        }

        public void ResetToInit()
        {
            colour = initVals.Colour;
            scaleMultiplier = def_scaleMultiplier;
            centrePositionOffset = initVals.Centre;
            centrePositionScaledOffset = def_cetrePposition_offset;
            positionOffset_BottomLeft = initVals.Bottom_Left;
            positionOffset_TopLeft = initVals.Top_Left;
            positionOffset_TopRight = initVals.Top_Right;
            positionOffset_BottomRight = initVals.Bottom_Right;
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

            Vector2 bottom_Left = positionOffset_BottomLeft * scaleMultiplier;
            Vector2 top_Left = positionOffset_TopLeft * scaleMultiplier; 
            Vector2 top_Right = positionOffset_TopRight * scaleMultiplier; 
            Vector2 bottom_Right = positionOffset_BottomRight * scaleMultiplier;

            float height = Mathf.Abs(bottom_Left.y - top_Right.y);
            float width = Mathf.Abs(bottom_Left.x - bottom_Right.x);
            Vector2 centreOffset = centrePositionScaledOffset * new Vector2(width, height);

            bottom_Left = Vector2Extension.Rotate(bottom_Left, rotationDegrees);
            top_Left = Vector2Extension.Rotate(top_Left, rotationDegrees);
            top_Right = Vector2Extension.Rotate(top_Right, rotationDegrees);
            bottom_Right = Vector2Extension.Rotate(bottom_Right, rotationDegrees);

            bottom_Left += centreOffset;
            top_Left += centreOffset;
            top_Right += centreOffset;
            bottom_Right += centreOffset;
            bottom_Left += centrePositionOffset;
            top_Left += centrePositionOffset;
            top_Right += centrePositionOffset;
            bottom_Right += centrePositionOffset;

            verices[verticeIndex + 0] = bottom_Left;
            verices[verticeIndex + 1] = top_Left;
            verices[verticeIndex + 2] = top_Right;
            verices[verticeIndex + 3] = bottom_Right;
        }
    }

}
