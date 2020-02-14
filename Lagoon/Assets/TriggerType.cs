using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerType : MonoBehaviour
{

    public enum TRIGGER_TYPE
    {
        FISHING,
        RADIO,
        SLEEP
    }

    [SerializeField] TRIGGER_TYPE type;

    public TRIGGER_TYPE GetTrigger()
    {
        return type;
    }

}
