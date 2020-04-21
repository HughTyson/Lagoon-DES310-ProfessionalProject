using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PagePair_Inventory : BasePagePair
{
    [SerializeField] BasePagePair goBackPair;


    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton to_stats;

    [SerializeField] TextMeshProUGUI left_text_box;
    [SerializeField] TextMeshProUGUI right_text_box;

    [SerializeField] SpecialText.SpecialText special_text_left;
    [SerializeField] SpecialText.SpecialText special_text_right;

    [SerializeField] PagePair_Stats stats_pair;

    SpecialText.SpecialTextData left_special_text = new SpecialText.SpecialTextData();
   SpecialText.SpecialTextData right_special_text = new SpecialText.SpecialTextData();

    int left_character_data = 0;
    int right_character_data = 0;

    InventoryItem[] items;

    private void Awake()
    {
        goBackButton.Event_Selected += requestGoBack;
        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });

        to_stats.Event_Selected += requestGoTo_Stats;
        to_stats.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.RB });

    }

    private void Start()
    {
    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();    //show the go back button
        to_stats.Show();

        //clear the text_boxes
        left_text_box.text = "";
        right_text_box.text = "";

        left_character_data = 0;
        right_character_data = 0;

        left_special_text = new SpecialText.SpecialTextData();
        right_special_text = new SpecialText.SpecialTextData();

        items = GM_.Instance.inventory.items.ToArray();

        for (int i = 0; i < GM_.Instance.inventory.items.Count; i++)     //loop for the amount of items in the inventory
        {

            if(GM_.Instance.inventory.items[i].GetItemType() == InventoryItem.ItemType.GENERIC)
            {
                AddToLeft(i);
            }
            else if(GM_.Instance.inventory.items[i].GetItemType() == InventoryItem.ItemType.SPECIFIC)
            {
                AddToRight(i);
            }
        }

        special_text_left.Begin(left_special_text);
        special_text_right.Begin(right_special_text);

    }
    public override void FinishedEnteringPage()
    {
        goBackButton.ListenForSelection();
        to_stats.ListenForSelection();
    }
    void requestGoBack()
    {

            Invoke_EventRequest_CloseJournal();
    }

    public override void FinishedExitingPage()
    {
        
    }

    void AddToLeft(int i)
    {

        int new_data_len = GM_.Instance.inventory.items[i].GetName().Length + " x".Length + items[i].GetQuantity().ToString().Length + " - ".Length + System.Environment.NewLine.Length + items[i].GetDescription().Length + System.Environment.NewLine.Length;

        if (GM_.Instance.inventory.items[i].isNew())     //if the item is new
        {

            left_special_text.AddCharacterData(GM_.Instance.inventory.items[i].GetName() + " x" + items[i].GetQuantity() + " - " + System.Environment.NewLine + items[i].GetDescription() + System.Environment.NewLine);
            left_special_text.AddPropertyToText(
                    new List<SpecialText.TextProperties.Base>()
                    {

                            new SpecialText.TextProperties.Colour(218,165,32),
                            new SpecialText.TextProperties.StaticAppear(),
                            new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                    },
                    left_character_data,
                    new_data_len
                    );

            left_character_data += new_data_len;

            GM_.Instance.inventory.items[i].SetIsNew(false);   //set the item to not new

        }
        else if (!GM_.Instance.inventory.items[i].isNew())   //if the items is not new
        {
            left_special_text.AddCharacterData(GM_.Instance.inventory.items[i].GetName() + " x" + items[i].GetQuantity() + " - " + System.Environment.NewLine + items[i].GetDescription() + System.Environment.NewLine);
            left_special_text.AddPropertyToText(
                    new List<SpecialText.TextProperties.Base>()
                    {
                            new SpecialText.TextProperties.Colour(255,255,255),
                            new SpecialText.TextProperties.StaticAppear()
                    },
                    left_character_data,
                    new_data_len
                    );

            left_character_data += new_data_len;
        }
    }

    void AddToRight(int i)
    {

        int new_data_len = GM_.Instance.inventory.items[i].GetName().Length + " x".Length + items[i].GetQuantity().ToString().Length + " - ".Length + System.Environment.NewLine.Length + items[i].GetDescription().Length + System.Environment.NewLine.Length;

        if (GM_.Instance.inventory.items[i].isNew())     //if the item is new
        {

            right_special_text.AddCharacterData(GM_.Instance.inventory.items[i].GetName() + " x" + items[i].GetQuantity() + " - " + System.Environment.NewLine + items[i].GetDescription() + System.Environment.NewLine);
            right_special_text.AddPropertyToText(
                    new List<SpecialText.TextProperties.Base>()
                    {

                            new SpecialText.TextProperties.Colour(218,165,32),
                            new SpecialText.TextProperties.StaticAppear(),
                            new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                    },
                    right_character_data,
                    new_data_len
                    );

            right_character_data += new_data_len;

            GM_.Instance.inventory.items[i].SetIsNew(false);   //set the item to not new

        }
        else if (!GM_.Instance.inventory.items[i].isNew())   //if the items is not new
        {
            right_special_text.AddCharacterData(GM_.Instance.inventory.items[i].GetName() + " x" + items[i].GetQuantity() + " - " + System.Environment.NewLine + items[i].GetDescription() + System.Environment.NewLine);
            right_special_text.AddPropertyToText(
                    new List<SpecialText.TextProperties.Base>()
                    {
                            new SpecialText.TextProperties.Colour(255,255,255),
                            new SpecialText.TextProperties.StaticAppear()
                    },
                    right_character_data,
                    new_data_len
                    );

            right_character_data += new_data_len;
        }

    }

    void requestGoTo_Stats()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(stats_pair));
    }
}
