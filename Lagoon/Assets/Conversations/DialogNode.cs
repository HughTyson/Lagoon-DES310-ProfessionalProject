using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ConversationCharacter: ScriptableObject    
{ 

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



public class RootNode : XNode.Node
{
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;
}



[NodeWidth(304)]
public class DialogNode : XNode.Node
{
   
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
}




[NodeWidth(304)]
public class BranchingNode : XNode.Node
{

    
    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;

    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int outputA;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int outputB;

    [TextArea(4, 6)]
    public string DecisionA;
    [TextArea(4, 6)]
    public string DecisionB;

}


[NodeWidth(304)]
public class EventNode : XNode.Node
{
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
public class BarrierNode : XNode.Node
{
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