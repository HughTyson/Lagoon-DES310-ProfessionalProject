﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Options : BasePagePair
{

    [SerializeField] UnselectableButton goBackButton;



    void Awake()
    {
        goBackButton.Event_Selected += request_GoBack;
        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });
    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();
    }

    void request_GoBack()
    {
        Invoke_EventRequest_GoToPreviousPage();
    }

}
