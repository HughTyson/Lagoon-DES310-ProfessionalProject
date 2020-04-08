using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Apple");
        SetItemType(ItemType.GENERIC);
        SetDescription("A delicious snack");
        SetQuanity(Random.Range(1, 7));
        setBaseCommonality(45.0f);
        SetUpdate(true);
    }
}

public class Chocolate : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Chocolate");
        SetItemType(ItemType.GENERIC);
        SetDescription("A sugar rush");
        SetQuanity(1);
        setBaseCommonality(5.0f);
        SetUpdate(true);
    }
}

public class SunTanLotion : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("SunTan Lotion");
        SetItemType(ItemType.GENERIC);
        SetDescription("Factor 50, it's hot");
        SetQuanity(1);
        setBaseCommonality(10.0f);
        SetUpdate(true);
    }
}

public class Puzzle : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Puzzle");
        SetItemType(ItemType.GENERIC);
        SetDescription("A way to pass the time");
        SetQuanity(1);
        setBaseCommonality(30.0f);
        SetUpdate(true);
    }
}

public class RemoteControlHelicopter : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("RemoteControl Helicopter");
        SetItemType(ItemType.GENERIC);
        SetDescription("A fun way to pass the time");
        SetQuanity(1);
        setBaseCommonality(0.1f);
        SetUpdate(true);
    }
}

public class SwitchItem: InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Switch");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("Will help fix the plane");
        SetQuanity(1);
        SetUpdate(true);
    }
}

public class Wrench : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Wrench");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("A tool to fix the plane");
        SetQuanity(1);
        SetUpdate(true);
    }
}

public class ScrewDriver : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("ScrewDriver");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("A tool to fix the plane");
        SetQuanity(1);
        SetUpdate(true);
    }
}

public class Salt : InventoryItem
{
    // Start is called before the first frame update
    public override void Init()
    {
        SetName("Salt");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("For a bit of flavour");
        SetQuanity(1);
        SetUpdate(true);
    }
}