using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Inventory : BasePagePair
{

    [SerializeField] UnselectableButton goBackButton;

    private void Awake()
    {
        goBackButton.Event_Selected += requestGoBack;
        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });

    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();
    }
    void requestGoBack()
    {
        Invoke_EventRequest_GoToPreviousPage();
    }

    public override void FinishedExitingPage()
    {
        GM_.Instance.inventory.SetNewItemsToNonNew();
    }

}
