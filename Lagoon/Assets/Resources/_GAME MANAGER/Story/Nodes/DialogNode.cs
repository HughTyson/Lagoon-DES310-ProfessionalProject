using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(304)]
public class DialogNode : CharacterHolderNode
{
    [System.Serializable]
    public class DialogStruct
    {
        public enum Talking
        {
            Left,
            Right
        }

        [NodeEnum]
        public Talking whoIsTalking;
        [TextArea(2, 6)]
        public string dialog_text;
    }


    protected override void Init()
    {
        node_type = NODE_TYPE.DIALOG;
    }

    public List<DialogStruct> Dialog = new List<DialogStruct>();
    [Input(ShowBackingValue.Never,ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;


    public void AddDialogStruct()
    {
        Dialog.Add(new DialogStruct());
    }

    public void Swap(int from_index, int to_index)
    {
        DialogStruct temp = Dialog[to_index];
        Dialog[to_index] = Dialog[from_index];
        Dialog[from_index] = temp;
    }

    int index;
    public void ResetDialogIndex()
    {
        index = 0;
    }
    public DialogStruct GetCurrentDialog()
    {
        return Dialog[index];
    }
    public bool IsOnLastDialog()
    {
        return (index == Dialog.Count - 1);
    }
    public DialogStruct IterateAndGetDialog()
    {
        index++;
        return Dialog[index];
    }

    public BaseNodeType NextNode()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }

}













