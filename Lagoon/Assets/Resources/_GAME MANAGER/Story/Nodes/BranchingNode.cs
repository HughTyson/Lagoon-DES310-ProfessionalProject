using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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


    public string LeftDecision;
    public string RightDecision;

    public float nodeWidth = 300;

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