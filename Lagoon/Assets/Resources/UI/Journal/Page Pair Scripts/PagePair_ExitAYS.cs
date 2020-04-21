using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PagePair_ExitAYS : BasePagePair
{
    [SerializeField] SelectableButton_ yesButton;
    [SerializeField] SelectableButton_ noButton;

    [SerializeField] SelectableAndUnhoverableButton goBackButton;

    [SerializeField] BasePagePair goBackPair;

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
        goBackButton.ListenForSelection();
        yesButton.HoverOver();
    }

    public override void BegunExitingPage()
    {
            noButton.SafeUnHoverOver();
            yesButton.SafeUnHoverOver();
    }

    void closeDownToMainMenu()
    {
        GM_.Instance.scene_manager.ChangeScene(0);
    }

    void request_GoBack()
    {
           Invoke_EventRequest_ChangePage(new RequestToChangePage(goBackPair));
    }



}
