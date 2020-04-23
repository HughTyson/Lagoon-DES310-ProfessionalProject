using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using TMPro;

public class InventoryWrapper : MonoBehaviour
{

    public Image inventory_image;
    [SerializeField] TextMeshProUGUI number_of_objects;
    [SerializeField] TextMeshProUGUI item_name;

    public SpecialText.SpecialTextData item_name_specialtext = new SpecialText.SpecialTextData();
    public SpecialText.SpecialTextData item_number_specialtext = new SpecialText.SpecialTextData();

    [SerializeField] SpecialText.SpecialText special_text_names;
    [SerializeField] SpecialText.SpecialText special_text_numbers;

    public void Hide()
    {
        inventory_image.enabled = false;
        number_of_objects.enabled = false;
        item_name.enabled = false;
    }

    public void Show()
    {
        inventory_image.enabled = true;
        number_of_objects.enabled = true;
        item_name.enabled = true;
    }

    public void Clear()
    {
        item_name_specialtext = new SpecialText.SpecialTextData();
        item_number_specialtext = new SpecialText.SpecialTextData();

        number_of_objects.text = "";
        item_name.text = "";
        //inventory_image = null;

    }

    public void BeginSpecialTexts()
    {
        special_text_names.Begin(item_name_specialtext);
        special_text_numbers.Begin(item_number_specialtext);
    }

}

[System.Serializable]
public class ItemSprite
{
    public InventoryItem.ItemType type;
    public Sprite sprite;
}
