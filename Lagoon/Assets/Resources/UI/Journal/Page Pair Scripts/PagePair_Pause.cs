using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Pause : BasePagePair
{
    [SerializeField] SelectableButton resumeButton;
    [SerializeField] SelectableButton optionsButton;
    [SerializeField] SelectableButton exitButton;
    [SerializeField] UnselectableButton exitToJournalButton;

    [SerializeField] BasePagePair pagePair_gameOptions;
    [SerializeField] BasePagePair pagePair_AreYouSure_Exit;



    void Awake()
    {
        resumeButton.Event_Selected += request_PutAwayBook;
        exitToJournalButton.Event_Selected += request_PutAwayBook;

        optionsButton.Event_Selected += request_GoToOptionsPair;
        exitButton.Event_Selected += request_GoToAreYouSurePair;

        exitToJournalButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B});
    }


    private void Start()
    {

    }


    public override void BegunEnteringPage()
    {
        resumeButton.Show();
        optionsButton.Show();
        exitButton.Show();
        exitToJournalButton.Show();
    }
    public override void FinishedEnteringPage()
    {
        InfoRequest_CameraFromPage_Args info = Invoke_InfoRequest_CameFromPage();

        if (info.pageType == typeof(PagePair_Options))
        {
            optionsButton.HoveredOver();
        }
        else if (info.pageType == typeof(PagePair_ExitAYS))
        {
            exitButton.HoveredOver();
        }
        else
        {
            resumeButton.HoveredOver();
        }
    }


    public override void FinishedExitingPage()
    {

    }



    void request_PutAwayBook()
    {
        Invoke_EventRequest_CloseJournal();
    }

  
    void request_GoToAreYouSurePair()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(pagePair_AreYouSure_Exit));
    }
    void request_GoToOptionsPair()
    {     
        Invoke_EventRequest_ChangePage(new RequestToChangePage(pagePair_gameOptions));
    }



}
