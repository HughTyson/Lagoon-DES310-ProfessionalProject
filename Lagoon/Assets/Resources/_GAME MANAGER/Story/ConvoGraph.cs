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

    
    public GlobalPropertiesNode GlobalProperties { 
        get {
            if (globalPropertiesNode == null)
            {
                FindGlobalPropertiesNode();
            }
            return globalPropertiesNode; 
        } 
    }
    public RootNode Root { get { 
            if (rootNode == null)
            {
                FindRootNode();
            }
            return rootNode; } 
    }

    public void FindRootNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (((BaseNodeType)nodes[i]).GetNodeType() == BaseNodeType.NODE_TYPE.ROOT)
            {
                rootNode = (RootNode)nodes[i];
                return;
            }
        }
        rootNode = null;
    }

    public void FindGlobalPropertiesNode()
    {
        for (int i = 0; i < nodes.Count; i++)
        {
            if (((BaseNodeType)nodes[i]).GetNodeType() == BaseNodeType.NODE_TYPE.GLOBAL_PROPERTIES)
            {
                globalPropertiesNode = (GlobalPropertiesNode)nodes[i];
                return;
            }
        }
        globalPropertiesNode =  null;
    }
}