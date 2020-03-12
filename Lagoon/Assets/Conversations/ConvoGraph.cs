using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

[CreateAssetMenu]
public class ConvoGraph : NodeGraph
{
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
}