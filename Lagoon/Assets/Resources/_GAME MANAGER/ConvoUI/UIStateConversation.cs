using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateConversation : MonoBehaviour
{
    // Start is called before the first frame update

    //Portraits 
    void Start()
    {
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.DIALOG_STARTED, Dialog_Started);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.DIALOG_UPDATE, Dialog_Update);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BRANCH_STARTED, Branch_Started);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BRANCH_UPDATE, Branch_Upate);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BRANCH_CHOICE_CHANGE, Branch_ChoiceChange);
        GM_.Instance.story.actionHandler.AddAction(StoryManager.ACTION.BARRIER_START, Barrier_Start);
    }

    void Dialog_Started()
    {

    }
    void Dialog_Update()
    {

    }
    void Branch_Started()
    {

    }
    void Branch_Upate()
    {

    }
    void Branch_ChoiceChange()
    {

    }
    void Barrier_Start()
    {

    }


   
}
