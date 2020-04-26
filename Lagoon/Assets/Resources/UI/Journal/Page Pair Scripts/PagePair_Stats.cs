using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PagePair_Stats : BasePagePair
{

    [SerializeField] SelectableAndUnhoverableButton go_back_button;
    [SerializeField] SelectableAndUnhoverableButton to_inventory;
    [SerializeField] SelectableAndUnhoverableButton to_tutorial;
    

    [SerializeField] TextMeshProUGUI stats_left_page;
    [SerializeField] TextMeshProUGUI stats_right_page;


    [SerializeField] PagePair_Inventory inventory_pair;
    [SerializeField] PagePair_Tutorial tutorial_pair;

    private void Awake()
    {
        go_back_button.Event_Selected += request_PutAwayBook;
        go_back_button.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });

        to_inventory.Event_Selected += requestGoTo_Inventory;
        to_inventory.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.LB });

        to_tutorial.Event_Selected += requestGoTo_Tutorials;
        to_tutorial.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });
    }


    // Start is called before the first frame update
    void Start()
    {
    }

    public override void BegunEnteringPage()
    {

        go_back_button.Show();
        to_inventory.Show();
        to_tutorial.Show();

        stats_left_page.text = "";
        stats_right_page.text = "";

        stats_left_page.text += "Days Passed - " + ((GM_.Instance.stats.dayNumber) + 13) + System.Environment.NewLine + System.Environment.NewLine;

        stats_left_page.text += "My biggest fish: " + System.Environment.NewLine;
        stats_left_page.text += "Type - " + GM_.Instance.stats.bigestFishStats.type + System.Environment.NewLine;
        stats_left_page.text += "Size - " + GM_.Instance.stats.bigestFishStats.size.ToString("F2") + " meters " + System.Environment.NewLine;
        stats_left_page.text += "Satisfaction: " + GM_.Instance.stats.bigestFishStats.satisfaction + System.Environment.NewLine + System.Environment.NewLine;

        stats_right_page.text += "Last fish caught: " + System.Environment.NewLine;
        stats_right_page.text += "Type - " + GM_.Instance.stats.last_fish_stats.type + System.Environment.NewLine;
        stats_right_page.text += "Size - " + GM_.Instance.stats.last_fish_stats.size.ToString("F2") + " meters " + System.Environment.NewLine;
        stats_right_page.text += "Satisfaction: " + GM_.Instance.stats.last_fish_stats.satisfaction + System.Environment.NewLine;

        

    }

    public override void FinishedEnteringPage()
    {
        go_back_button.ListenForSelection();
        to_inventory.ListenForSelection();
        to_tutorial.ListenForSelection();
    }

    void request_PutAwayBook()
    {
        Invoke_EventRequest_CloseJournal();
    }

    void requestGoTo_Inventory()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(inventory_pair));
    }

    void requestGoTo_Tutorials()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(tutorial_pair));
    }
}
