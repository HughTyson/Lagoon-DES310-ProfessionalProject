using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class ConvoGraph : NodeGraph
{

    GlobalPropertiesNode globalPropertiesNode;
    RootNode rootNode;
    public override Node AddNode(Type type)
    {

        Node node = base.AddNode(type);

        if (((BaseNodeType)node) == null)
        {
            base.RemoveNode(node);
            return null;
        }
        switch (((BaseNodeType)node).GetNodeType())
        {
            case BaseNodeType.NODE_TYPE.ROOT:
            {

                    break;
            }
        
        }

        return node; 
    }
    public override Node CopyNode(Node original)
    {
        return base.CopyNode(original);
    }
    public override void RemoveNode(Node node)
    {
        base.RemoveNode(node);
    }

    public BaseNodeType FindRootNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (((BaseNodeType)nodes[i]).GetNodeType() == BaseNodeType.NODE_TYPE.ROOT)
            {
                return (BaseNodeType)nodes[i];
            }
        }

        Debug.LogError("No 'RootNode' found in ConvoGraph!");
        return null;
    }

    public GlobalPropertiesNode FindGlobalPropertiesNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (((BaseNodeType)nodes[i]).GetNodeType() == BaseNodeType.NODE_TYPE.GLOBAL_PROPERTIES)
            {
                return (GlobalPropertiesNode)nodes[i];
            }
        }
        Debug.LogError("No 'GlobalPropertiesNode' found in ConvoGraph!");
        return null;
    }
}