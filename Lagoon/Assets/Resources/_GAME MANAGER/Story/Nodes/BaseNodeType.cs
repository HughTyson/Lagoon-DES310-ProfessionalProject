using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    [Input(connectionType = ConnectionType.Multiple, backingValue = ShowBackingValue.Never)]
    public int additionalInfo;


     XNode.NodePort additionalInfoPort = null;

    public override void OnCreateConnection(XNode.NodePort from, XNode.NodePort to)
    {
        if (to.fieldName == "additionalInfo") // this object's port
        {
            if (!typeof(BaseAdditionalInfoNode).IsAssignableFrom(from.node.GetType())) // only allow types of BaseAdditionalInfo and iheritters off.
            {
                to.Disconnect(from);
            }
        }
    }


    protected override void Init()
    {
        node_type = NODE_TYPE.BASE;
    }

    protected List<BaseAdditionalInfoNode.AdditionalInfo> travellingAdditionalInfo = new List<BaseAdditionalInfoNode.AdditionalInfo>();


    public BaseNodeType NextNode() 
    {
        BaseNodeType next_node = NextNode_Internal();

        next_node.SetGivenAdditionalInfo(travellingAdditionalInfo);
        return next_node;
    }


    protected List<BaseAdditionalInfoNode.AdditionalInfo> GetMyAdditionalInfoList()
    {
        List<BaseAdditionalInfoNode.AdditionalInfo> additionalInfoList = new List<BaseAdditionalInfoNode.AdditionalInfo>();

        GetPort("additionalInfo").GetConnections().ForEach(y => additionalInfoList.Add(((BaseAdditionalInfoNode)(y.node)).GetAdditionalInfo()));

        return additionalInfoList;
    }

    protected void SetGivenAdditionalInfo(List<BaseAdditionalInfoNode.AdditionalInfo> given_info)
    {
        travellingAdditionalInfo = given_info;
        given_info.AddRange(GetMyAdditionalInfoList());
    }


    protected virtual BaseNodeType NextNode_Internal()
    {
        return null;
    }

    public virtual List<BaseAdditionalInfoNode.AdditionalInfo> TakeUsedAdditionalInfo()
    {
        return new List<BaseAdditionalInfoNode.AdditionalInfo>();
    }
}
