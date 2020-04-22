using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
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


    public enum SpwanType
    {
        GENERIC,    //this item can be randomly picked
        SPECIFIC    //this item will only be dropped after a certain conversation event
    }

    public enum ItemType
    {
        APPLE,
        CHOCOLATE,
        FIREWOOD,
        SUNTANLOT,
        PUZZLE,
        REMOTECONTROLHELICOPTER,
        SWITCH,
        WRENCH,
        SCREWDRIVER,
        SALT
    }

    string item_name;
    string description;
    int quantiy;

    float base_commonality;
    float relative_commonality;

    Sprite sprite;

    SpwanType spawn_type;
    ItemType item_type;

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

    public void SetSpawnType(SpwanType type_)
    {
        spawn_type = type_;
    }

    public SpwanType GetSpawnType()
    {
        return spawn_type;
    }

    public void SetIsNew(bool is_updated_)
    {
        is_updated = is_updated_;
    }

    public bool isNew()
    {
        return is_updated;
    }


    public void SetItemType(ItemType type_)
    {
        item_type = type_;
    }

    public ItemType GetItemType()
    {
        return item_type;
    }

    public void SetItemImage(Sprite sprite_)
    {
        sprite = sprite_;
    }

    public Sprite GetItemImage()
    {
        return sprite;
    }
}