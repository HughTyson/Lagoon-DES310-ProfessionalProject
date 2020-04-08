using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PagePair_ExitAYS : BasePagePair
{
    [SerializeField] SelectableButton_ yesButton;
    [SerializeField] SelectableButton_ noButton;

    [SerializeField] UnselectableButton goBackButton;



    void Awake()
    {
        yesButton.Event_Selected += closeDownToMainMenu;
        noButton.Event_Selected += request_GoBack;
        goBackButton.Event_Selected += request_GoBack;
        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
    }



    public override void BegunEnteringPage()
    {
        noButton.Show();
        yesButton.Show();
        goBackButton.Show();
    }
    public override void FinishedEnteringPage()
    {
        yesButton.HoverOver();
    }

    public override void BegunExitingPage()
    {
        if (noButton.SelectableState == Selectable_.SELECTABLE_STATE.HOVERED_OVER)
        {
            noButton.UnHoverOver();
        }
        else if (yesButton.SelectableState == Selectable_.SELECTABLE_STATE.HOVERED_OVER)
        {
            yesButton.UnHoverOver();
        }
    }

    void closeDownToMainMenu()
    {
        // Create scene manager for Game Manager which cleans up game manager when scene changed (e.i, unpause, clear all tweens, apply loading screen, ect.)
       // SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
    }

    void request_GoBack()
    {
        Invoke_EventRequest_GoToPreviousPage();
    }



}
