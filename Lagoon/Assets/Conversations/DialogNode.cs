using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Dialog : ScriptableObject
{
    public enum SPOKEN_BY
    {
        LEFT,
        RIGHT
    };

    [TextArea]
    public string text;
    public SPOKEN_BY spoken_by; 
}


    public class DialogNode : XNode.Node
    {
        public Dialog dialog;
    }

