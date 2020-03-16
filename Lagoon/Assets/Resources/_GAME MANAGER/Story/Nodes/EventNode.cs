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