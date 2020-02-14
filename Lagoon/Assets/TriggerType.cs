using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerType : MonoBehaviour
{
    enum TRIGGER_TYPE
    {
        FISHING,
        RADIO
    }

    [SerializeField] TRIGGER_TYPE type;



    TRIGGER_TYPE GetTrigger()
    {
        return type;
    }

}
