using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFootAudioTriggerLogic : MonoBehaviour
{
    [SerializeField] CharacterFootAudioTrigger audioTriggerAsset;
    // Start is called before the first frame update
    public CharacterFootAudioTrigger AudioTrigger => audioTriggerAsset;
}
