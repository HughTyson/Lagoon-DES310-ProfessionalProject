using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagePair_Tutorial : BasePagePair
{

    [SerializeField] SelectableAndUnhoverableButton go_back_button;
    [SerializeField] SelectableAndUnhoverableButton to_stats;

    [SerializeField] PagePair_Stats stats_pair;

    // Start is called before the first frame update
    void Start()
    {

        go_back_button.Event_Selected += request_PutAwayBook;
        go_back_button.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });


        to_stats.Event_Selected += requestGoTo_Stats;
        to_stats.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });


        go_back_button.GroupWith(to_stats);
    }
    public override void BegunEnteringPage()
    {
        go_back_button.Show();
        to_stats.Show();

    }
    public override void FinishedEnteringPage()
    {
        go_back_button.ListenForSelection();
        to_stats.ListenForSelection();
    }
    public override void PassingBy()
    {
        go_back_button.Show();
        to_stats.Show();
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
