using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Options : BasePagePair
{

    [SerializeField] UnselectableButton goBackButton;
    [SerializeField] SelectableButton_TextButton game_SButton;
    [SerializeField] SelectableButton_TextButton control_SButton;
    [SerializeField] SelectableButton_TextButton audio_SButton;
    [SerializeField] SelectableButton_TextButton back_SButton;


    [SerializeField] BasePagePair gameOptionPair;
    [SerializeField] BasePagePair audioOptionPair;
    [SerializeField] BasePagePair controlOptionPair;

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
    }
    public override void FinishedEnteringPage()
    {
        InfoRequest_CameraFromPage_Args args = Invoke_InfoRequest_CameFromPage();

        //if (args.pageType == )
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
        Invoke_EventRequest_GoToPreviousPage();
    }

}
