using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


[NodeWidth(400)]
[NodeTint(122,255,122)]
[CreateNodeMenuAttribute("Additional Info - Certain Supply Drop Item")]
public class AdditionalInfoNode_CertainItemsInNextSupplyDrop : BaseAdditionalInfoNode
{
    private void Awake()
    {
        name = "Additional Info - Certain Supply Drop Item";
    }

    [System.Serializable]
    public class Items
    {
        public string s_ammount= "";
        public string s_type = "";

        int ammount;
        System.Type type;


        bool flaggedToRemove = false;
        public bool FlaggedToRemove => flaggedToRemove;
        public bool TryParseType()
        {
            return (InventoryItem.InventoryItemsTypes_Dictionary.TryGetValue(s_type, out type));
        }
        public bool TryParseAmmount()
        {
            bool allowable = false;
            int return_value;
            allowable = int.TryParse(s_ammount, out return_value);

            if (allowable)
            {
                if (return_value >= 0)
                {
                    ammount = return_value;
                    return true;
                }
                return false;
            }

            return false;
        }

        public ItemData ParseAndGetItemData()
        {
            TryParseType();
            TryParseAmmount();
            return new ItemData(type, ammount);
        }
        public void FlagToRemove()
        {
            flaggedToRemove = true;
        }    
    }
    public class DebugInfo
    {
         List<string> errors = new List<string>();

        public IReadOnlyList<string> Errors => errors;
        public void AddError(string msg)
        {
            errors.Add(msg);
        }

    }

    public DebugInfo DebugParse()
    {
        DebugInfo debugInfo = new DebugInfo();
        for (int i = 0; i < items.Count; i++)
        {
            if (!items[i].TryParseType())
            {
                debugInfo.AddError("Error at item " + i.ToString() + ": Type. Type not found. Did you mean:" + System.Environment.NewLine +
                    "   '"+ InventoryItem.MostSimilarInventoryItemName(items[i].s_type) + "'?");
            }
            if (!items[i].TryParseAmmount())
            {
                debugInfo.AddError("Error at item " + i.ToString() + ": Amount." +System.Environment.NewLine + 
                    "   Amount has to be an integer and positive.");
            }
        }
        return debugInfo;
    }


    public void RemoveFlaggedItems()
    {
        items.RemoveAll(
            y =>
            {
                return y.FlaggedToRemove;
            }
            );
    }
    public List<Items> items = new List<Items>();

    public struct ItemData
    {
        public ItemData(System.Type type_, int ammount_)
        {
            type = type_;
            ammount = ammount_;
        }
        public readonly System.Type type;
        public readonly int ammount;
    
    }

    public class CertainNextSupplyDropItems : AdditionalInfo
    {
        public readonly List<ItemData> items = new List<ItemData>();

        public CertainNextSupplyDropItems(List<Items> unparsedItems)
        {
            unparsedItems.ForEach(y => items.Add(y.ParseAndGetItemData()));
        }

    }


    public override AdditionalInfo GetAdditionalInfo()
    {
        CertainNextSupplyDropItems certainNextSupplyDropItems = new CertainNextSupplyDropItems(items);
        return certainNextSupplyDropItems;
    }
}
