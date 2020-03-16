using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(304)]
public class BranchingNode : BaseNodeType
{
    public enum CHOICE
    { 
        LEFT,
        RIGHT
    }

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


    public BaseNodeType NextNode(CHOICE chosen_choice)
    {
        switch (chosen_choice)
        {
            case CHOICE.LEFT:
                {
                    return (BaseNodeType)GetPort("outputA").Connection.node;
                }
            case CHOICE.RIGHT:
                {
                    return (BaseNodeType)GetPort("outputB").Connection.node;
                }
        
        }
        return null;

    }
    

}