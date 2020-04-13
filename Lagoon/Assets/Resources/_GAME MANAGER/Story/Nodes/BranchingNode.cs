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

    CHOICE? choice = null;

    public void SetChoice(CHOICE chosen_choice)
    {
        choice = (CHOICE?)chosen_choice;
    }
    protected override BaseNodeType NextNode_Internal()
    {
        switch (choice)
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