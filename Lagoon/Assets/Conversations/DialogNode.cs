using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class BaseNodeType : XNode.Node
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

    public virtual void LeftNode()
    {

    }

    public virtual void EnteredNode()
    {

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



public class RootNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.ROOT;
    }

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    public BaseNodeType NextNode()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }
}



[NodeWidth(304)]
public class DialogNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.DIALOG;
    }

    public ConversationCharacter leftCharacter;
    public ConversationCharacter rightCharacter;

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


    private int current_dialog_index = -1;
    public override void EnteredNode()
    {
        current_dialog_index = 0;
    }

    public override void LeftNode()
    {
        current_dialog_index = 0;
    }

    public DialogStruct GetDialog()
    {
        return Dialog[current_dialog_index];
    }
    public bool HasMoreDialog()
    {
        return current_dialog_index != Dialog.Count - 1;
    }
    public void IterateDialog()
    {
        current_dialog_index++;
    }
}




[NodeWidth(304)]
public class BranchingNode : BaseNodeType
{

    protected override void Init()
    {
        node_type = NODE_TYPE.BRANCH;
    }

    public ConversationCharacter leftCharacter;
    public ConversationCharacter rightCharacter;

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
public class BarrierNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.BARRIER;
    }

    public enum BARRIER_STATE
    {
        GO_TO_HUT,
        NEXT_DAY,
        CATCH_A_FISH
    }

    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    [NodeEnum]
    public BARRIER_STATE barrier;

}