using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeWidth(304)]
public class EventNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.EVENT;
    }

    public enum EVENT_TYPE
    {
        SUPPLY_DROP,
        SOMETHING
    }


    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    [NodeEnum]
    public EVENT_TYPE event_occured;

    public BaseNodeType NextNode()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }
}