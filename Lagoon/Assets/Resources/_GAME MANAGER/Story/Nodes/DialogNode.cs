using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(304)]
public class DialogNode : CharacterHolderNode
{
    [System.Serializable]
    public class DialogData
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

    public List<DialogData> Dialog = new List<DialogData>();
    [Input(ShowBackingValue.Never,ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;


    public void AddDialogStruct()
    {
        Dialog.Add(new DialogData());
    }

    public void Swap(int from_index, int to_index)
    {
        DialogData temp = Dialog[to_index];
        Dialog[to_index] = Dialog[from_index];
        Dialog[from_index] = temp;
    }

    int index;
    public void ResetDialogIndex()
    {
        index = 0;
    }


   
    public DialogData GetCurrentDialog()
    {
        return Dialog[index];
    }
    public bool IsOnLastDialog()
    {
        return (index == Dialog.Count - 1);
    }
    public DialogData IterateAndGetDialog()
    {
        index++;
        return Dialog[index];
    }

    public BaseNodeType NextNode()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }

}













