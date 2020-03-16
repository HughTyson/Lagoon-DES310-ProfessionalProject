using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(304)]
public class BarrierNode : RootNode
{
    protected override void Init()
    {
        node_type = NODE_TYPE.BARRIER;
    }

    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;

}



