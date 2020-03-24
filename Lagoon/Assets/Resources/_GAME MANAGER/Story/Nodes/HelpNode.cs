using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[NodeWidth(600)]
public class HelpNode : BaseNodeType
{ // all code is in the node editor
    protected override void Init()
    {
        node_type = NODE_TYPE.HELP;
    }
}
