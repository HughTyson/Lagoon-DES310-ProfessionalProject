using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpecialText
{
    public abstract class TextPropertyData_Base
    {
        public enum TYPE
        { 
            COLOUR,
            SHIVER
        }
        protected TYPE data_type;
        public TYPE DataType { get { return data_type; } }

        public abstract void Lex(Token property, List<Token> parameters, Action<string, Token> ErrorMessage);
    }

    public class TextPropertyData_Colour : TextPropertyData_Base
    {
        Color colour = new Color(0,0,0,1);
        public TextPropertyData_Colour()
        {
            data_type = TYPE.COLOUR;
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
    public class TextPropertyData_Shiver : TextPropertyData_Base
    {
        float shake_ammount = 0;
        public TextPropertyData_Shiver()
        {
            data_type = TYPE.SHIVER;
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
