using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Apple");
        SetItemType(ItemType.GENERIC);
        SetDescription("A delicious snack");
        SetQuanity((i == null ? Random.Range(1, 7) : (int)i));
        setBaseCommonality(45.0f);
        SetIsNew(true);
    }
}

public class Chocolate : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Chocolate");
        SetItemType(ItemType.GENERIC);
        SetDescription("A sugar rush");
        SetQuanity((i == null ? 1 : (int)i));
        setBaseCommonality(5.0f);
        SetIsNew(true);
    }
}

public class SunTanLotion : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("SunTan Lotion");
        SetItemType(ItemType.GENERIC);
        SetDescription("Factor 50, it's hot");
        SetQuanity((i == null ? 1 : (int)i));
        setBaseCommonality(10.0f);
        SetIsNew(true);
    }
}

public class Puzzle : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Puzzle");
        SetItemType(ItemType.GENERIC);
        SetDescription("A way to pass the time");
        SetQuanity((i == null ? 1 : (int)i));
        setBaseCommonality(30.0f);
        SetIsNew(true);
    }
}

public class RemoteControlHelicopter : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("RemoteControl Helicopter");
        SetItemType(ItemType.GENERIC);
        SetDescription("A fun way to pass the time");
        SetQuanity((i == null ? 1 : (int)i));
        setBaseCommonality(0.1f);
        SetIsNew(true);
    }
}

public class SwitchItem: InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Switch");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("Will help fix the plane");
        SetQuanity((i == null ? 1 : (int)i));
        SetIsNew(true);
    }
}

public class Wrench : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Wrench");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("A tool to fix the plane");
        SetQuanity((i == null ? 1 : (int)i));
        SetIsNew(true);
    }
}

public class ScrewDriver : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("ScrewDriver");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("A tool to fix the plane");
        SetQuanity((i == null ? 1 : (int)i));
        SetIsNew(true);
    }
}

public class Salt : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Salt");
        SetItemType(ItemType.SPECIFIC);
        SetDescription("For a bit of flavour");
        SetQuanity((i == null ? 1 : (int)i));
        SetIsNew(true);
    }
}