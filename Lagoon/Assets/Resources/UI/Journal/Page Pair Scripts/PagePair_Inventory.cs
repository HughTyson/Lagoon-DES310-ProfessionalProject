using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PagePair_Inventory : BasePagePair
{
    [SerializeField] BasePagePair goBackPair;


    [SerializeField] SelectableAndUnhoverableButton goBackButton;
    [SerializeField] SelectableAndUnhoverableButton to_stats;


    [SerializeField] List<InventoryWrapper> left_page_items;
    [SerializeField] List<InventoryWrapper> right_page_items;

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

        HideItems();

    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();    //show the go back button
        to_stats.Show();

        int left_used = 0;
        int right_used = 0;

        

        for (int i = 0; i < GM_.Instance.inventory.items.Count; i++)
        {

            if (GM_.Instance.inventory.items[i].GetSpawnType() == InventoryItem.SpwanType.GENERIC)
            {
                Debug.Log(GM_.Instance.inventory.items[i].isNew());
                left_page_items[left_used].Show();
                left_page_items[left_used].Clear();


                AddToLeftBoxes(i, left_used);
                left_page_items[left_used].BeginSpecialTexts();

                GM_.Instance.inventory.items[i].SetIsNew(false);

                left_used++;
            }
        }

        for (int i = 0; i < GM_.Instance.inventory.items.Count; i++)
        {
            if (GM_.Instance.inventory.items[i].GetSpawnType() == InventoryItem.SpwanType.SPECIFIC)
            {

                right_page_items[right_used].Show();
                right_page_items[right_used].Clear();

                AddToRightBoxes(i, right_used);
                right_page_items[right_used].BeginSpecialTexts();

                right_used++;
            }
        }

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
        for(int i = 0; i < left_page_items.Count; i++)
        {
            left_page_items[i].Clear();
        }
    }

    void AddToRightBoxes(int i, int right_used)
    {

        if (GM_.Instance.inventory.items[i].isNew())     //if the item is new
        {
            right_page_items[right_used].item_name_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetName());
            right_page_items[right_used].item_name_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {

                                new SpecialText.TextProperties.Colour(218,165,32),
                                new SpecialText.TextProperties.StaticAppear(),
                                new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                },
                0,
                GM_.Instance.inventory.items[i].GetName().Length
            );

            right_page_items[right_used].item_number_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetQuantity().ToString());
            right_page_items[right_used].item_number_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {

                        new SpecialText.TextProperties.Colour(218,165,32),
                        new SpecialText.TextProperties.StaticAppear(),
                        new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                },
                0,
                GM_.Instance.inventory.items[i].GetQuantity().ToString().Length
            );

            right_page_items[i].inventory_image.sprite = GM_.Instance.inventory.items[i].GetItemImage();

            GM_.Instance.inventory.items[i].SetIsNew(false);

        }
        else if (!GM_.Instance.inventory.items[i].isNew())
        {
            right_page_items[right_used].item_name_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetName());
            right_page_items[right_used].item_name_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {
                                new SpecialText.TextProperties.Colour(255,255,255),
                                new SpecialText.TextProperties.StaticAppear()
                },
                0,
                GM_.Instance.inventory.items[i].GetName().Length
            );

            right_page_items[right_used].item_number_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetQuantity().ToString());
            right_page_items[right_used].item_number_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {
                                new SpecialText.TextProperties.Colour(255,255,255),
                                new SpecialText.TextProperties.StaticAppear()
                },
                0,
                GM_.Instance.inventory.items[i].GetQuantity().ToString().Length
            );
        }

    }

    void AddToLeftBoxes(int i, int left_used)
    {

        if (GM_.Instance.inventory.items[i].isNew())     //if the item is new
        {
            left_page_items[left_used].item_name_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetName());
            left_page_items[left_used].item_name_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {

                                new SpecialText.TextProperties.Colour(218,165,32),
                                new SpecialText.TextProperties.StaticAppear(),
                                new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                },
                0,
                GM_.Instance.inventory.items[i].GetName().Length
            );

            left_page_items[left_used].item_number_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetQuantity().ToString());
            left_page_items[left_used].item_number_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {

                        new SpecialText.TextProperties.Colour(218,165,32),
                        new SpecialText.TextProperties.StaticAppear(),
                        new SpecialText.TextProperties.WaveScaled(1,0.5f,5)
                },
                0,
                GM_.Instance.inventory.items[i].GetQuantity().ToString().Length
            );

            left_page_items[left_used].inventory_image.sprite = GM_.Instance.inventory.items[i].GetItemImage();

            GM_.Instance.inventory.items[i].SetIsNew(false);

        }
        else if (!GM_.Instance.inventory.items[i].isNew())
        {
            left_page_items[i].item_name_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetName());
            left_page_items[i].item_name_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {
                                new SpecialText.TextProperties.Colour(255,255,255),
                                new SpecialText.TextProperties.StaticAppear()
                },
                0,
                GM_.Instance.inventory.items[i].GetName().Length
            );

            left_page_items[i].item_number_specialtext.CreateCharacterData(GM_.Instance.inventory.items[i].GetQuantity().ToString());
            left_page_items[i].item_number_specialtext.AddPropertyToText(
                new List<SpecialText.TextProperties.Base>()
                {
                                new SpecialText.TextProperties.Colour(255,255,255),
                                new SpecialText.TextProperties.StaticAppear()
                },
                0,
                GM_.Instance.inventory.items[i].GetQuantity().ToString().Length
            );
        }
    }


    void requestGoTo_Stats()
    {
        Invoke_EventRequest_ChangePage(new RequestToChangePage(stats_pair));
    }

    void HideItems()
    {

        for (int i = 0; i < left_page_items.Count; i++)
        {
            left_page_items[i].Hide();
        }

        for (int i = 0; i < right_page_items.Count; i++)
        {
            right_page_items[i].Hide();
        }
    }
}
