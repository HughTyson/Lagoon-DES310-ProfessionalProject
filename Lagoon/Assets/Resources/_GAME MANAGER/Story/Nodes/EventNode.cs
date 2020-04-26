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

    public enum EVENT_TYPE
    {
        SUPPLY_DROP,
        NEXT_SCENE,
        SLEEP,
        SOMETHING
    }


    [Input(ShowBackingValue.Never, ConnectionType.Multiple)] public int input;
    [Output(ShowBackingValue.Never, ConnectionType.Override)] public int output;

    [NodeEnum]
    public EVENT_TYPE event_occured;

    protected override BaseNodeType NextNode_Internal()
    {
        return (BaseNodeType)GetPort("output").Connection.node;
    }

    public override List<BaseAdditionalInfoNode.AdditionalInfo> TakeUsedAdditionalInfo()
    {
        List<BaseAdditionalInfoNode.AdditionalInfo> output = new List<BaseAdditionalInfoNode.AdditionalInfo>();
        switch (event_occured)
        {
            case EVENT_TYPE.SUPPLY_DROP:
                {
                    travellingAdditionalInfo.RemoveAll(
                        y =>
                        {
                            if (y.GetType() == typeof(AdditionalInfoNode_CertainItemsInNextSupplyDrop.CertainNextSupplyDropItems))
                            {
                                output.Add(y);
                                return true;
                            }
                            return false;
                        }
                        );
                    break;
                }
        
        }

        return output;
    }
}