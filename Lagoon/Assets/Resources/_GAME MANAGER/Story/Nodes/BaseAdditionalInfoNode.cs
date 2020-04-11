using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XNode;

public abstract class BaseAdditionalInfoNode : BaseNodeType
{
    [Output(connectionType = ConnectionType.Override)]
    public int output;


    public abstract AdditionalInfo GetAdditionalInfo();
    public abstract class AdditionalInfo
    { 
    
    
    };

}
