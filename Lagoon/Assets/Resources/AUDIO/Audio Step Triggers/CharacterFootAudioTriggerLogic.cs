using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootAudioTriggerLogic : MonoBehaviour
{
    [SerializeField] CharacterFootAudioTrigger audioTriggerAsset;

    [SerializeField] bool overideDefaultPrioirity = false;

    [Range(0,255)]
    [SerializeField] int overridenPriorityVal;
    // Start is called before the first frame update
    public CharacterFootAudioTrigger AudioTrigger => audioTriggerAsset;
    
    public int Priority()
    {
        if (overideDefaultPrioirity)
        {
            return overridenPriorityVal;
        }
        else
        {
            return audioTriggerAsset.DefaultMultipleTriggeredPriority;
        }
    }

   
}
