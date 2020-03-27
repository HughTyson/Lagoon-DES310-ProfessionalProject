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


    void Start()
    {
        GM_.Instance.story.Event_ConvoCharactersShow += ConvoCharactersShow;
        GM_.Instance.story.Event_DialogStart += DialogStart;
        GM_.Instance.story.Event_DialogNewText += DialogNewText;
        GM_.Instance.story.Event_BranchChoiceMade += BranchChoiceChosen;
        GM_.Instance.story.Event_BranchStart += BranchStart;
        GM_.Instance.story.Event_SkipTextCrawl += SkipTextCrawl;
        GM_.Instance.story.Event_ConvoExit += ConversationExit;
        GM_.Instance.story.EventRequest_BlockingButtonPress += ShouldBlockButtons;
        GM_.Instance.story.Event_GameEventTriggered += GameEventTriggered;

        dialogBox.Event_BoxFinishedAppearing += dialogStartTextShouldShowIterate;

        leftPortrait.Event_FinishedChanging += dialogStartTextShouldShowIterate;
        rightPortrait.Event_FinishedChanging += dialogStartTextShouldShowIterate;

        leftPortrait.Event_FinishedAppearing += dialogStartTextShouldShowIterate;
        rightPortrait.Event_FinishedAppearing += dialogStartTextShouldShowIterate;



    }



    void ConversationExit()
    {
        leftPortrait.Disappear();
        rightPortrait.Disappear();
        if (dialogBox.IsBoxShowing())
            dialogBox.Disappear();
    }

    void  ConvoCharactersShow(StoryManager.ConvoCharactersShowArgs args)
    {
        leftCharacter = args.leftCharacter;
        rightCharacter = args.rightCharacter;

        leftPortrait.Appear(leftCharacter);
        rightPortrait.Appear(rightCharacter);
    }

    void GameEventTriggered(StoryManager.GameEventTriggeredArgs args)
    {

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


    void SkipTextCrawl()
    {
        if (leftPortrait.IsTransitioning())
            leftPortrait.SkipTransition();

        if (rightPortrait.IsTransitioning())
            rightPortrait.SkipTransition();

        if (dialogBox.IsTransitioning())
            dialogBox.SkipTransition();

        if (choiceBoxes.IsTransitioning())
            choiceBoxes.SkipTransition();
    }

    void ShouldBlockButtons(StoryManager.ButtonPressRequestArgs request)
    {       
        if (leftPortrait.IsTransitioning())
        {
            request.Block();
            return;
        }
        if (rightPortrait.IsTransitioning())
        {
            request.Block();
            return;
        }
        if (dialogBox.IsTransitioning())
        {
            request.Block();
            return;
        }
        if (choiceBoxes.IsTransitioning())
        {
            request.Block();
            return;
        }
    }
}
