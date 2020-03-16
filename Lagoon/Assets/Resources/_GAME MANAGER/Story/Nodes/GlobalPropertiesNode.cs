using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(400)]
public class GlobalPropertiesNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.GLOBAL_PROPERTIES;
    }


    public float DefaultSpeedPerTextCharacter = 0.1f;
}
