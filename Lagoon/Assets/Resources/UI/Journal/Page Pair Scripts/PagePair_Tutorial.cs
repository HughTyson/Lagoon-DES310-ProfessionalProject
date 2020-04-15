using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Tutorial : BasePagePair
{

    [SerializeField] UnselectableButton go_back_button;
    [SerializeField] UnselectableButton to_stats;

    [SerializeField] PagePair_Stats stats_pair;

    // Start is called before the first frame update
    void Start()
    {

        go_back_button.Event_Selected += request_PutAwayBook;
        go_back_button.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });


        to_stats.Event_Selected += requestGoTo_Stats;
        to_stats.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });

    }

    void requestGoTo_Stats()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(stats_pair));
    }

    void request_PutAwayBook()
    {
        Invoke_EventRequest_CloseJournal();
    }
}
