using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class PagePair_Inventory : BasePagePair
{

    [SerializeField] UnselectableButton goBackButton;

    [SerializeField] TextMeshProUGUI left_text_box;
    //[SerializeField] TextMeshProUGUI right_text_box;
    [SerializeField] SpecialText.SpecialText special_text;

   

    private void Awake()
    {
        goBackButton.Event_Selected += requestGoBack;
        goBackButton.SetButtonsToCheckForPress(new InputManager.BUTTON[] { InputManager.BUTTON.B });

    }

    private void Start()
    {
    }

    public override void BegunEnteringPage()
    {
        goBackButton.Show();    //show the go back button
        Show();


        //text_box.text = "";

        ///text_box.text += GM_.Instance.inventory.items[0].GetName() + System.Environment.NewLine;// + " x" + GM_.Instance.inventory.items[i].GetQuantity() + " - " + GM_.Instance.inventory.items[i].GetDescription() + System.Environment.NewLine;
        //special_text.Begin(text_data);

        //clear the text_boxes
        left_text_box.text = "";     
        //right_text_box.text = "";

        int left_character_data = 0;
        int right_character_data = 0;

        SpecialText.SpecialTextData left_special_text = new SpecialText.SpecialTextData();
        SpecialText.SpecialTextData right_special_text = new SpecialText.SpecialTextData();

        InventoryItem[] items = GM_.Instance.inventory.items.ToArray();


        for (int i = 0; i < items.Length; i++)     //loop for the amount of items in the inventory
        {

            if (items[i].isNew())     //if the item is new
            {

                //left_special_text.CreateCharacterData(items[i].GetName());
                //left_special_text.AddPropertyToText(
                //        new List<SpecialText.TextProperties.Base>()
                //        {

                //            new SpecialText.TextProperties.Colour(0,255,0),
                //            new SpecialText.TextProperties.AppearAtOnce(),
                //            new SpecialText.TextProperties.WaveScaled(1,2,5)
                //        },
                //        character_data,
                //        items[i].GetName().Length
                //    );

                left_character_data += items[i].GetName().Length;

                left_text_box.text += items[i].GetName() + System.Environment.NewLine;// + " x" + items[i].GetQuantity() + " - " + items[i].GetDescription() + System.Environment.NewLine;

                GM_.Instance.inventory.items[i].SetUpdate(false);   //set the item to not new

            }
            else if (!items[i].isNew())   //if the items is not new
            {
                if(items[i].GetItemType() == InventoryItem.ItemType.GENERIC)
                {
                    left_character_data += items[i].GetName().Length;
                    left_text_box.text += items[i].GetName() + " x" + items[i].GetQuantity() + " - " + items[i].GetDescription() + "\n";
                }
                else if(items[i].GetItemType() == InventoryItem.ItemType.SPECIFIC)
                {
                    right_character_data += items[i].GetName().Length;
                    //right_text_box.text += items[i].GetName() + " x" + items[i].GetQuantity() + " - " + items[i].GetDescription() + "\n";
                }

            }
        }

        //special_text.Begin(special_text_data_new);

    }

    void requestGoBack()
    {
        Invoke_EventRequest_GoToPreviousPage();
    }

    public override void FinishedExitingPage()
    {
        
    }

    public void Hide()
    {
       // text_box.SetActive(false);
    }

    public void Show()
    {
       // text_box.SetActive(true);
    }
}
