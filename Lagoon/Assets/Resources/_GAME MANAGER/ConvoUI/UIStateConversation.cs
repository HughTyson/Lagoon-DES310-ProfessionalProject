using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIStateConversation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] UICharacterPortrait leftPortrait;
    [SerializeField] UICharacterPortrait rightPortrait;
    [SerializeField] UIDialogBox dialogBox;
    [SerializeField] UIChoiceBoxes choiceBoxes;

    ConversationCharacter leftCharacter;
    ConversationCharacter rightCharacter;

    SpecialText.SpecialTextData latestSpecialText;
    DialogNode.DialogData.Talking latestWhosTalking;


    void Awake()
    {
        GM_.Instance.story.Event_ConvoCharactersShow += ConvoCharactersShow;
        GM_.Instance.story.Event_DialogStart += DialogStart;
        GM_.Instance.story.Event_DialogNewText += DialogNewText;
        GM_.Instance.story.Event_BranchChoiceMade += BranchChoiceChosen;
        GM_.Instance.story.Event_BranchStart += BranchStart;
        GM_.Instance.story.Event_ConvoExit += ConversationExit;

        GM_.Instance.story.EventRequest_BlockingButtonPress += ShouldBlockButtons;
        GM_.Instance.story.Event_GameEventStart += GameEventTriggered;




        dialogBox.Event_BoxFinishedAppearing += dialogStartTextShouldShowIterate;

        leftPortrait.Event_FinishedChanging += dialogStartTextShouldShowIterate;
        rightPortrait.Event_FinishedChanging += dialogStartTextShouldShowIterate;

        leftPortrait.Event_FinishedAppearing += dialogStartTextShouldShowIterate;
        rightPortrait.Event_FinishedAppearing += dialogStartTextShouldShowIterate;



    }



    int countOfCompletedTransitions;

    void ConversationExit_CompletedTransition()
    {
        countOfCompletedTransitions--;

        if (countOfCompletedTransitions == 0)
        {
            leftPortrait.Event_FinishedDisappearing -= ConversationExit_CompletedTransition;
            rightPortrait.Event_FinishedDisappearing -= ConversationExit_CompletedTransition;
            dialogBox.Event_Dissapeared -= ConversationExit_CompletedTransition;
            GM_.Instance.story.EventRequest_BarrierStart -= Blocker;
            GM_.Instance.story.RequestBarrierStart();
        }
    }
    void ConversationExit()
    {
        GM_.Instance.story.EventRequest_BarrierStart += Blocker;
        countOfCompletedTransitions = 2;

        leftPortrait.Event_FinishedDisappearing += ConversationExit_CompletedTransition;
        leftPortrait.Disappear();

        rightPortrait.Event_FinishedDisappearing += ConversationExit_CompletedTransition;
        rightPortrait.Disappear();

        if (dialogBox.IsBoxShowing())
        {
            countOfCompletedTransitions++;
            dialogBox.Event_Dissapeared += ConversationExit_CompletedTransition;
            dialogBox.Disappear();
        }
    }

    void Blocker(StoryManager.EventRequestArgs args)
    {
        args.Block();
    }

    void  ConvoCharactersShow(StoryManager.ConvoCharactersShowArgs args)
    {
        leftCharacter = args.leftCharacter;
        rightCharacter = args.rightCharacter;

        leftPortrait.NotTalking();
        rightPortrait.NotTalking();

        leftPortrait.Appear(leftCharacter);
        rightPortrait.Appear(rightCharacter);
    }

    void GameEventTriggered(StoryManager.GameEventTriggeredArgs args)
    {
        leftPortrait.NotTalking();
        rightPortrait.NotTalking();
        GM_.Instance.story.EventRequest_GameEventContinue += BlockingGameEventContinue;
        dialogBox.Event_Dissapeared += UnblockGameEvent;

        if (dialogBox.IsBoxShowing())
        {
            dialogBox.Disappear();
        }
        else
        {
            UnblockGameEvent();
        }
    }
    void BlockingGameEventContinue(StoryManager.EventRequestArgs args)
    {
        args.Block();
    }
    void UnblockGameEvent()
    {
        dialogBox.Event_Dissapeared -= UnblockGameEvent;
        GM_.Instance.story.EventRequest_GameEventContinue -= BlockingGameEventContinue;



        GM_.Instance.story.RequestGameEventContinue();
    }

    void DialogStart(StoryManager.DialogEnterArgs args)
    {
        if (!choiceBoxes.IsTransitioning())
        {

            dialog_start_text_iterator = 0;

            dialogBox.ClearContinueSymbol();

           
            if (leftCharacter != args.leftCharacter)
            {
                leftPortrait.ChangeCharacter(args.leftCharacter);                
                leftCharacter = args.leftCharacter;
            }
            else if (!leftPortrait.IsTransitioning())
            {
                dialogStartTextShouldShowIterate();
            }

            if (rightCharacter != args.rightCharacter)
            {
                rightPortrait.ChangeCharacter(args.rightCharacter);
                rightCharacter = args.rightCharacter;
            }
            else if (!rightPortrait.IsTransitioning())
            {
                dialogStartTextShouldShowIterate();
            }

            latestSpecialText = args.specialTextData;
            latestWhosTalking = args.whosTalking;

            if (!dialogBox.IsBoxShowing())
            {
                dialogBox.Appear();
            }
            else
            {
                dialogStartTextShouldShowIterate();
            }
        }
        else
        {
            unseen_dialog_enter_args = args;
            choiceBoxes.Event_FinishedSelection += branchFinishedAndEnteredDialog;
        }
    }


    StoryManager.DialogEnterArgs unseen_dialog_enter_args;
    void branchFinishedAndEnteredDialog()
    {
        choiceBoxes.Event_FinishedSelection -= branchFinishedAndEnteredDialog;

        dialog_start_text_iterator = 0;

        dialogBox.ClearContinueSymbol();

        if (leftCharacter != unseen_dialog_enter_args.leftCharacter)
        {
            leftPortrait.ChangeCharacter(unseen_dialog_enter_args.leftCharacter);
            leftCharacter = unseen_dialog_enter_args.leftCharacter;
        }
        else if (!leftPortrait.IsTransitioning())
        {
            dialogStartTextShouldShowIterate();
        }

        if (rightCharacter != unseen_dialog_enter_args.rightCharacter)
        {
            rightPortrait.ChangeCharacter(unseen_dialog_enter_args.rightCharacter);
            rightCharacter = unseen_dialog_enter_args.rightCharacter;
        }
        else if (!rightPortrait.IsTransitioning())
        {
            dialogStartTextShouldShowIterate();
        }

        latestSpecialText = unseen_dialog_enter_args.specialTextData;
        latestWhosTalking = unseen_dialog_enter_args.whosTalking;


        if (!dialogBox.IsBoxShowing())
        {
            dialogBox.Appear();
        }
        else
        {
            dialogStartTextShouldShowIterate();
        }
    }


    void BranchChoiceChosen(StoryManager.BranchChoiceMadeArgs args)
    {
        choiceBoxes.SelectOption(args.choice);
    }



    StoryManager.BranchEnterArgs lastBranchArgs;
    void BranchStart(StoryManager.BranchEnterArgs args)
    {
        lastBranchArgs = args;
        leftPortrait.Talking();
        rightPortrait.NotTalking();

        if (dialogBox.IsBoxShowing())
        {
            dialogBox.Event_Dissapeared += showBranchBox;
            dialogBox.Disappear();
        }
        else
        {
            showBranchBox();
        }
    }

    void showBranchBox()
    {
        dialogBox.Event_Dissapeared -= showBranchBox;
        choiceBoxes.Appear(lastBranchArgs);
    }




    int dialog_start_text_iterator = 0;
    void dialogStartTextShouldShowIterate()
    {
        dialog_start_text_iterator++;
        if (dialog_start_text_iterator == 3)
        {
            dialogBox.WriteText(latestSpecialText);

            if (latestWhosTalking == DialogNode.DialogData.Talking.Left)
            {
                leftPortrait.Talking();
                rightPortrait.NotTalking();
            }
            else if (latestWhosTalking == DialogNode.DialogData.Talking.Right)
            {
                leftPortrait.NotTalking();
                rightPortrait.Talking();
            }
        }
    }


    void DialogNewText(StoryManager.DialogNewTextArgs args)
    {
        if (args.whosTalking == DialogNode.DialogData.Talking.Left)
        {
            leftPortrait.Talking();
            rightPortrait.NotTalking();
        }
        else if (args.whosTalking == DialogNode.DialogData.Talking.Right)
        {
            leftPortrait.NotTalking();
            rightPortrait.Talking();
        }

        latestSpecialText = args.specialTextData;
        latestWhosTalking = args.whosTalking;

        dialogBox.WriteText(args.specialTextData);
    }


    void ShouldBlockButtons(StoryManager.ButtonPressRequestArgs request)
    {
        bool isTransitioning = false;

        if (leftPortrait.IsTransitioning())
        {
            isTransitioning = true;
        }
        if (rightPortrait.IsTransitioning())
        {
            isTransitioning = true;
        }
        if (dialogBox.IsTransitioning())
        {
            isTransitioning = true;
        }
        if (choiceBoxes.IsTransitioning())
        {
            isTransitioning = true;
        }

        if (isTransitioning)
        {
            if (request.RequestedButton == InputManager.BUTTON.A || request.RequestedButton == InputManager.BUTTON.X || request.RequestedButton == InputManager.BUTTON.B)
            {
                GM_.Instance.input.SetVibrationWithPreset(InputManager.VIBRATION_PRESET.MENU_BUTTON_PRESSED);
                leftPortrait.SkipTransition();
                rightPortrait.SkipTransition();
                dialogBox.SkipTransition();
                choiceBoxes.SkipTransition();
            }

            request.Block();
        }
    }
}
