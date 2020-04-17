using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PagePair_Stats : BasePagePair
{

    [SerializeField] UnselectableButton go_back_button;
    [SerializeField] UnselectableButton to_inventory;
    [SerializeField] UnselectableButton to_tutorial;
    

    [SerializeField] TextMeshProUGUI stats_box;
    [SerializeField] TextMeshProUGUI plane_box;


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

        stats_box.text = "";
        plane_box.text = "";

        stats_box.text += "Days Passed - " + ((GM_.Instance.stats.dayNumber) + 13) + System.Environment.NewLine + System.Environment.NewLine;

        stats_box.text += "My biggest fish: " + System.Environment.NewLine;
        stats_box.text += "Type - " + GM_.Instance.stats.bigestFishStats.type + System.Environment.NewLine;
        stats_box.text += "Size - " + GM_.Instance.stats.bigestFishStats.size + System.Environment.NewLine;
        stats_box.text += "Satisfaction: " + GM_.Instance.stats.bigestFishStats.satisfaction + System.Environment.NewLine + System.Environment.NewLine;

        stats_box.text += "Last fish caught: " + System.Environment.NewLine;
        stats_box.text += "Type - " + GM_.Instance.stats.last_fish_stats.type + System.Environment.NewLine;
        stats_box.text += "Size - " + GM_.Instance.stats.last_fish_stats.size.ToString("F2") + System.Environment.NewLine;
        stats_box.text += "Satisfaction: " + GM_.Instance.stats.last_fish_stats.satisfaction + System.Environment.NewLine;

        plane_box.text += "Plane Fixes" + System.Environment.NewLine;

        for (int i = 0; i < GM_.Instance.stats.plane_segments_stats.Count; i++)
        {
            plane_box.text += GM_.Instance.stats.plane_segments_stats[i].segment_name + " - ";

            if (!GM_.Instance.stats.plane_segments_stats[i].complete)
            {
                plane_box.text += "Not fixed" + System.Environment.NewLine;
            }
            else if (GM_.Instance.stats.plane_segments_stats[i].complete)
            {
                plane_box.text += "Fixed " + System.Environment.NewLine;
            }
        }

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
