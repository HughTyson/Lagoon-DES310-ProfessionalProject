using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Self Pointers")]
    public UIHelperButtons helperButtons;
    public UITransition transition;
    public UIStateFishVictory state_fishVictory;
    public UIStateConversation state_conversation;
    // Start is called before the first frame update
    

    public void ManagerUpdate()
    {
        transition.ManagerUpdate();
    }
}
