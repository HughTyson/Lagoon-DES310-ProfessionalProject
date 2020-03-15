using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateConversation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UICharacterPortrait leftPortrait;
    [SerializeField] UICharacterPortrait rightPortrait;
    [SerializeField] UIDialogBox dialogBox;

    ConversationCharacter leftCharacter;
    ConversationCharacter rightCharacter;

    DialogStruct latestDialog;

    void Start()
    {
        GM_.Instance.story.Event_ConvoEnter += ConvoEnter;
        GM_.Instance.story.Event_ConvoExit += ConvoExit;
        GM_.Instance.story.Event_ConvoCharactersShow += ConvoCharactersShow;
        GM_.Instance.story.Event_DialogStart += DialogStart;

        dialogBox.Event_BoxFinishedAppearing += dialogboxFinishedAppearing;
    }

    void ConvoEnter()
    {
        GM_.Instance.update_events.UpdateEvent += EventBasedUpdate;
    }

    void ConvoExit()
    {
        GM_.Instance.update_events.UpdateEvent -= EventBasedUpdate;
    }

    void  ConvoCharactersShow(StoryManager.ConvoCharactersShowArgs args)
    {
        leftCharacter = args.leftCharacter;
        rightCharacter = args.rightCharacter;

        leftPortrait.Appear(leftCharacter);
        rightPortrait.Appear(rightCharacter);
    }

    void DialogStart(StoryManager.DialogEnterArgs args)
    {
        latestDialog = args.dialogVars;

        dialogBox.Appear();
    }

    void dialogboxFinishedAppearing()
    {
        dialogBox.WriteText(latestDialog.dialog);
    }

    void EventBasedUpdate()
    {

    }
   
}
