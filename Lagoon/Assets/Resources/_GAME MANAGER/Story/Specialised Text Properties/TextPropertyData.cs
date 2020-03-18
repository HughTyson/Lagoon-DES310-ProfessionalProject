using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{

    public static class TextPropertyData
    {
        public enum DATA_TYPE
        {
            COLOUR,
            SHIVER,
            CHAR_SPEED
        }

        public static readonly Dictionary<string, DATA_TYPE> TypeDictionary = new Dictionary<string, DATA_TYPE>
        {
            { "colour" ,  DATA_TYPE.COLOUR},
            { "shiver" ,  DATA_TYPE.SHIVER},
            { "char_speed", DATA_TYPE.CHAR_SPEED}
        };

        public static Base ParseFromEnumToPropertyData(DATA_TYPE data_type)
        {
            switch (data_type)
            {
                case DATA_TYPE.COLOUR: return new Colour();
                case DATA_TYPE.SHIVER: return new Shiver();
                case DATA_TYPE.CHAR_SPEED: return new CharSpeed();
                default: return null;
            }
        }
        public static Base ParseNameToTextPropertyData(string name)
        {
            DATA_TYPE d_type;
            if (TypeDictionary.TryGetValue(name, out d_type))
            {
                return ParseFromEnumToPropertyData(d_type);
            }
            return null;
        }
        public static bool TryParseNameToTextPropertyData(string name, out Base property_data)
        {
            DATA_TYPE d_type;
            if (TypeDictionary.TryGetValue(name, out d_type))
            {
                property_data = ParseFromEnumToPropertyData(d_type);
                return true;
            }
            property_data = null;
            return false;
        }
        public static string FindMostSimilarPropertyName(string name)
        {
            string most_similar_name = "";
            int most_similar_value = int.MaxValue;
            foreach (string key in TypeDictionary.Keys)
            {
               int new_similarity = StringDistance.GetDamerauLevenshteinDistance(name, key);
                if (new_similarity < most_similar_value)
                {
                    most_similar_value = new_similarity;
                    most_similar_name = key;
                }
            }
            return most_similar_name;
        }

        public static void ApplyDefaults(ref List<Base> propertyDatas, GlobalPropertiesNode globalProperties)
        {
            if (!propertyDatas.Exists(y => { return (y.DataType == DATA_TYPE.CHAR_SPEED); }))
            {
                propertyDatas.Add(new CharSpeed(globalProperties.DefaultSpeedPerTextCharacter));
            }
        }


        public abstract class Base
        {

            protected DATA_TYPE data_type;
            public DATA_TYPE DataType { get { return data_type; } }

            public abstract void Lex(Token property, List<Token> parameters, Action<string, Token> ErrorMessage);
            public virtual void CheckForIncompatibles(List<Base> propertyList, Action<string, Token> ErrorMessage)
            {
                propertyList.Remove(this);
            }
        }

        public class Colour : Base
        {
            Color colour = new Color(0, 0, 0, 1);
            public Colour()
            {
                data_type = DATA_TYPE.COLOUR;
            }
            public override void Lex(Token property, List<Token> parameters, Action<string, Token> ErrorMessage)
            {
                if (parameters.Count != 3)
                {
                    ErrorMessage("Error: Incorrect number of parameters. Parameter number: 3", property);
                    return;
                }
                else
                {
                    float[] vals = new float[parameters.Count];
                    for (int i = 0; i < parameters.Count; i++)
                    {
                        if (parameters[i].Data == "")
                        {
                            ErrorMessage("Error: Incorrect number of parameters. Parameter number: 3", property);
                        }
                        if (!float.TryParse(parameters[i].Data, out vals[i]))
                        {
                            ErrorMessage("Error: Incorrect parameter type. Parameter must be a float.", parameters[i]);
                            return;
                        }

                    }
                    colour.r = vals[0];
                    colour.g = vals[1];
                    colour.b = vals[2];
                }
            }
        }
        public class Shiver : Base
        {
            float shake_ammount = 0;
            public Shiver()
            {
                data_type = DATA_TYPE.SHIVER;
            }
            public override void Lex(Token property, List<Token> parameters, Action<string, Token> ErrorMessage)
            {
                if (parameters.Count != 1)
                {
                    ErrorMessage("Error: Incorrect number of parameters. Parameter number: 1", property);
                    return;
                }
                else
                {
                    float val;
                    if (!float.TryParse(parameters[0].Data, out val))
                    {
                        ErrorMessage("Error: Incorrect parameter type. Parameter must be a float.", parameters[0]);
                        return;
                    }
                    if (val < 0)
                    {
                        ErrorMessage("Error: parameter cannot be negative.", parameters[0]);
                        return;
                    }
                }
            }
        }
        public class CharSpeed : Base
        {
            float speed;
            public CharSpeed()
            {
                data_type = DATA_TYPE.CHAR_SPEED;
            }
            public CharSpeed(float speed_)
            {
                data_type = DATA_TYPE.CHAR_SPEED;
                speed = speed_;
            }
            public override void Lex(Token property, List<Token> parameters, Action<string, Token> ErrorMessage)
            {
                if (parameters.Count != 1)
                {
                    ErrorMessage("Error: Incorrect number of parameters. Parameter number: 1", property);
                    return;
                }
                else
                {
                    float val;
                    if (!float.TryParse(parameters[0].Data, out val))
                    {
                        ErrorMessage("Error: Incorrect parameter type. Parameter must be a float.", parameters[0]);
                        return;
                    }
                    if (val < 0)
                    {
                        ErrorMessage("Error: parameter cannot be negative.", parameters[0]);
                        return;
                    }
                }
            }
        }
    }
   
}
