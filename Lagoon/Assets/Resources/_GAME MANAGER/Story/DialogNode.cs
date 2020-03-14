using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public abstract class BaseNodeType : XNode.Node
{
    public enum NODE_TYPE
    {
        BASE,
        ROOT,
        DIALOG,
        BRANCH,
        EVENT,
        BARRIER
    };

    protected NODE_TYPE node_type;
    public NODE_TYPE GetNodeType() { return node_type; }

    protected override void Init()
    {
        node_type = NODE_TYPE.BASE;
    }

}



[System.Serializable]
public struct DialogStruct
{
    public enum Talking
    {
        Left,
        Right
    }

    [NodeEnum]
    public Talking whoIsTalking;
    [TextArea(4, 6)]
    public string dialog;
}




[NodeWidth(304)]
public abstract class CharacterHolderNode : BaseNodeType
{
    public ConversationCharacter leftCharacter;
    public ConversationCharacter rightCharacter;
}


[NodeWidth(304)]
public class DialogNode : CharacterHolderNode
{
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

}




[NodeWidth(304)]
public class BranchingNode : CharacterHolderNode
{

    protected override void Init()
    {
        node_type = NODE_TYPE.BRANCH;
    }

    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int outputA;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int outputB;

    [TextArea(4, 6)]
    public string LeftDecision;
    [TextArea(4, 6)]
    public string RightDecision;

}


[NodeWidth(304)]
public class EventNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.EVENT;
    }

    public enum EVENT_STATE
    { 
    SUPPLY_DROP,
    SOMETHING    
    }


    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    [NodeEnum]
    public EVENT_STATE event_occured;
}


[NodeWidth(304)]
public class BarrierNode : RootNode
{
    protected override void Init()
    {
        node_type = NODE_TYPE.BARRIER;
    }

    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;

}

[NodeWidth(304)]
public class RootNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.ROOT;
    }
    public enum BARRIER_STATE
    {
        NEXT_DAY,
        CATCH_A_FISH
    }

    [NodeEnum]
    public List<BARRIER_STATE> barriers;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    public BaseNodeType NextNode()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }
}