using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public abstract class InventoryItem
{
    static Dictionary<string,System.Type> _inventoryItemTypes_Dictionary = null;
    public static Dictionary<string, System.Type> InventoryItemsTypes_Dictionary
    { 
        get
        {
            if (_inventoryItemTypes_Dictionary == null)
            {
                _inventoryItemTypes_Dictionary = ReflectiveEnumerator.GetSubClassesOfType<InventoryItem>().ToDictionary(key => key.Name, data => data);
            }
            return _inventoryItemTypes_Dictionary;
        }
    }

    public static string MostSimilarInventoryItemName(string input)
    {
        string mostSimilar = "";
        int mostResemblenceValue = int.MaxValue;
        foreach (KeyValuePair<string, System.Type> pair in InventoryItemsTypes_Dictionary)
        {
            int resemblenceVal = StringDistance.GetLevenshteinDistance(input, pair.Key);
            if (resemblenceVal < mostResemblenceValue)
            {
                mostResemblenceValue = resemblenceVal;
                mostSimilar = pair.Key;
            }
        }
        return mostSimilar;
    }


    public enum ItemType
    {
        GENERIC,    //this item can be randomly picked
        SPECIFIC    //this item will only be dropped after a certain conversation event
    }

    string item_name;
    string description;
    int quantiy;

    float base_commonality;
    float relative_commonality;

    ItemType type;

    bool is_updated = false;

    public virtual void Init(int? i = null) { }

    public void SetName(string n)
    {
        item_name = n;
    }

    public string GetName()
    {
        return item_name;
    }

    public void SetDescription(string d)
    {
        description = d;
    }

    public string GetDescription()
    {
        return description;
    }

    public void SetQuanity(int amount)
    {
        quantiy = amount;
    }

    public int GetQuantity()
    {
        return quantiy;
    }

    public void setBaseCommonality(float commonality)
    {
        base_commonality = commonality;
    }

    public float GetBaseCommonality()
    {
        return base_commonality;
    }

    public void setRelativeCommonality(float commonality)
    {
        relative_commonality = commonality;
    }

    public float GetRelativeCommonality()
    {
        return relative_commonality;
    }

    public void SetItemType(ItemType type_)
    {
        type = type_;
    }

    public ItemType GetItemType()
    {
        return type;
    }

    public void SetIsNew(bool is_updated_)
    {
        is_updated = is_updated_;
    }

    public bool isNew()
    {
        return is_updated;
    }

}