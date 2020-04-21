using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Options : BasePagePair
{

    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableButton_TextButton game_SButton;
    [SerializeField] SelectableButton_TextButton control_SButton;
    [SerializeField] SelectableButton_TextButton audio_SButton;
    [SerializeField] SelectableButton_TextButton back_SButton;


    [SerializeField] PagePair_OptionsGame gameOptionPair;
    [SerializeField] PagePair_OptionsAudio audioOptionPair;
    [SerializeField] PagePair_OptionsControl controlOptionPair;

    [SerializeField] BasePagePair goBackPair;
    void Awake()
    {
        goBackButton.Event_Selected += request_GoBack;
        back_SButton.Event_Selected += request_GoBack;

        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });


        game_SButton.Event_Selected += requestGoTo_GameOptions;
        control_SButton.Event_Selected += requestGoTo_ControlOptions;
        audio_SButton.Event_Selected += requestGoTo_AudioOptions;


    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();
        game_SButton.Show();
        control_SButton.Show();
        audio_SButton.Show();
        back_SButton.Show();
    }


    public override void FinishedEnteringPage()
    {
        goBackButton.ListenForSelection();

        InfoRequest_CameraFromPage_Args args = Invoke_InfoRequest_CameFromPage();

        if (args.pageType == typeof(PagePair_OptionsGame))
        {
            game_SButton.HoverOver();
        }
        else if (args.pageType == typeof(PagePair_OptionsAudio))
        {
            audio_SButton.HoverOver();
        }
        else if (args.pageType == typeof(PagePair_OptionsControl))
        {
            control_SButton.HoverOver();
        }
        else
        {
            game_SButton.HoverOver();
        }
    }

    public override void BegunExitingPage()
    {
            control_SButton.SafeUnHoverOver();
            audio_SButton.SafeUnHoverOver();
            back_SButton.SafeUnHoverOver();
            game_SButton.SafeUnHoverOver();
    }


    void requestGoTo_GameOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(gameOptionPair));
    }
    void requestGoTo_AudioOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(audioOptionPair));
    }
    void requestGoTo_ControlOptions()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(controlOptionPair));
    }

    void request_GoBack()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(goBackPair));
    }

}
