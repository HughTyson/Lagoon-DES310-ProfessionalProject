using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeWidth(400)]
public class GlobalPropertiesNode : BaseNodeType
{
    public event System.Action ValuesChanged;
    protected override void Init()
    {
        node_type = NODE_TYPE.GLOBAL_PROPERTIES;
    }


    public float DefaultSpeedPerTextCharacter = 20.0f;
}
