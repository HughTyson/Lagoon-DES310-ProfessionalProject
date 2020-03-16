using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseNodeType : XNode.Node
{
    public enum NODE_TYPE
    {
        BASE,
        ROOT,
        DIALOG,
        BRANCH,
        EVENT,
        BARRIER,
        GLOBAL_PROPERTIES,
        HELP
    };

    protected NODE_TYPE node_type;
    public NODE_TYPE GetNodeType() { return node_type; }

    protected override void Init()
    {
        node_type = NODE_TYPE.BASE;
    }

}
