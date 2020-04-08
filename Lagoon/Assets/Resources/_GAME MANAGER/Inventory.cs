﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Reflection;
using System.Linq;

public class Inventory
{



    [HideInInspector] public List<InventoryItem> items = new List<InventoryItem>();

    readonly List<InventoryItem> item_instances;
    readonly float accumulated_weight;

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

            if(temp_item.GetItemType() == InventoryItem.ItemType.GENERIC)   //only use generics as they will have to be included in the commonanilty calculations
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
                unkown_item = true;
                break;
            }
        }

        if(!unkown_item)
        {
            items.Add(new_item);
        }
    }

    public void AddNewContent()
    {

    }

    public bool SearchFor(System.Type type)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[0].GetType() == type)
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


}
