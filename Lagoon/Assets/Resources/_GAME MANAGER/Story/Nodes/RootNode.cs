using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[NodeWidth(304)]
public class RootNode : BaseNodeType
{
    protected override void Init()
    {
        node_type = NODE_TYPE.ROOT;      
    }
    public enum BARRIER_STATE
    {
        NEXT_DAY,
        CATCH_A_FISH,
        END
    }

    [NodeEnum]
    public List<BARRIER_STATE> barriers = new List<BARRIER_STATE>();

    [Output(ShowBackingValue.Never, ConnectionType.Override)]  public int output;
    

    public BaseNodeType NextNode()
    {
        XNode.NodePort port = GetPort("output");
        if (port.Connection != null)
        {
            return (BaseNodeType)port.Connection.node;
        }


        return null;
    }

    public void AddBarrier()
    {
        barriers.Add(new BARRIER_STATE());
    }
    public void RemoveBarrier()
    {
        barriers.RemoveAt(barriers.Count - 1);
    }
}
