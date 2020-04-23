using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System.Linq;

public class Inventory
{



    [HideInInspector] public List<InventoryItem> items = new List<InventoryItem>();

    readonly List<InventoryItem> item_instances;
    readonly float accumulated_weight;

    bool updated_inventory = false;

    [HideInInspector] public List<ItemSprite> item_images = new List<ItemSprite>();

    public Inventory()
    {
        System.Type[] types = Assembly.GetExecutingAssembly().GetTypes();

        System.Type[] possible = (from System.Type type in types where type.IsSubclassOf(typeof(InventoryItem)) select type).ToArray();

        //item_instances = new InventoryItem[possible.Length];
        item_instances = new List<InventoryItem>();

        for (int i = 0; i < possible.Length; i++)
        {

            InventoryItem temp_item = (InventoryItem)System.Activator.CreateInstance(possible[i]);
            temp_item.Init();

            if(temp_item.GetSpawnType() == InventoryItem.SpwanType.GENERIC)   //only use generics as they will have to be included in the commonanilty calculations
            {

                item_instances.Add((InventoryItem)System.Activator.CreateInstance(possible[i]));

                item_instances[i].Init();
               
                accumulated_weight += item_instances[item_instances.Count - 1].GetBaseCommonality();
                item_instances[item_instances.Count - 1].setRelativeCommonality(accumulated_weight);
            }

        }
    }

    public void AddNewItem(InventoryItem new_item)
    {
        bool unkown_item = false;

        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].GetType() == new_item.GetType())
            {
                items[i].SetQuanity(items[i].GetQuantity() + new_item.GetQuantity());
                items[i].SetIsNew(true);
                unkown_item = true;
                break;
            }
        }

        if(!unkown_item)
        {
            new_item.SetItemImage(GetSprite(new_item.GetItemType()));
            items.Add(new_item);
        }

        updated_inventory = true;
    }

    public void AddNewContent()
    {
    }

    public bool SearchFor(System.Type type)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].GetType() == type)
            {
                return true;
            }
        }

        return false;
    }

    public System.Type GetRandomType()
    {

        float random = Random.Range(0, accumulated_weight);

        for(int i = 0; i < item_instances.Count; i++)
        {
            if(item_instances[i].GetRelativeCommonality() >= random)
            {
                return item_instances[i].GetType();
            }
        }

        return null; //this should only be call if there are no entires
    }

    public Sprite GetSprite(InventoryItem.ItemType type)
    {
        for(int i = 0; i < item_images.Count; i++)
        {
            if(item_images[i].type == type)
            {
                return item_images[i].sprite;
            }
        }

        return null;
    }

    public void ButtonPrompt()
    {
        if(updated_inventory)
        {
            GAME_UI.Instance.helperButtons.EnableButton(UIHelperButtons.BUTTON_TYPE.Y, "Journal");

            if(GM_.Instance.input.GetButtonDown(InputManager.BUTTON.Y))
            {
                updated_inventory = false;
                GAME_UI.Instance.helperButtons.DisableAll();
            }
        }
    }

    public void RemoveItemType(System.Type type)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if(items[i].GetType() == type)
            {
                items[i].SetQuanity(items[i].GetQuantity() - 1);

                if(items[i].GetQuantity() == 0)
                {
                    items.RemoveAt(i);
                }
            }
        }
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public void Reset()
    {

        items.Clear();


        Firewood wood = new Firewood();
        wood.Init(15);
        wood.SetItemImage(GetSprite(wood.GetItemType()));

        Puzzle puzzle = new Puzzle();
        puzzle.Init(2);
        puzzle.SetItemImage(GetSprite(puzzle.GetItemType()));

        Puzzle puzzle2 = new Puzzle();
        puzzle2.Init(3);
        puzzle2.SetItemImage(GetSprite(puzzle2.GetItemType()));


        ScrewDriver c = new ScrewDriver();
        c.Init();
        c.SetItemImage(GetSprite(c.GetItemType()));

        Chocolate d = new Chocolate();
        d.Init();
        d.SetItemImage(GetSprite(d.GetItemType()));

        items.Add(d);


        RemoteControlHelicopter r = new RemoteControlHelicopter();
        r.Init();
        r.SetItemImage(GetSprite(r.GetItemType()));

        SwitchItem i = new SwitchItem();
        i.Init();
        i.SetItemImage(GetSprite(i.GetItemType()));
        items.Add(i);


        items.Add(wood);
        items.Add(puzzle);
        items.Add(c);
       
    }
}
