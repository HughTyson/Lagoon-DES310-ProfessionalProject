using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apple : InventoryItem
{
    // Start is called before the first frame update
    public override void Init(int? i = null)
    {
        SetName("Apple");
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.APPLE);
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
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.CHOCOLATE);
        SetDescription("A sugar rush");
        SetQuanity((i == null ? 1 : (int)i));
        setBaseCommonality(5.0f);
        SetIsNew(true);
    }
}

public class Firewood : InventoryItem
{
    public override void Init(int? i = null)
    {
        SetName("Firewood");
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.FIREWOOD);
        SetDescription("Sun of a birch");
        SetQuanity((i == null ? 10 : (int)i));
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
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.SUNTANLOT);
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
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.PUZZLE);
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
        SetSpawnType(SpwanType.GENERIC);
        SetItemType(ItemType.REMOTECONTROLHELICOPTER);
        SetDescription("They forgot to send batteries");
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
        SetSpawnType(SpwanType.SPECIFIC);
        SetItemType(ItemType.SWITCH);
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
        SetSpawnType(SpwanType.SPECIFIC);
        SetItemType(ItemType.WRENCH);
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
        SetSpawnType(SpwanType.SPECIFIC);
        SetItemType(ItemType.SCREWDRIVER);
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
        SetSpawnType(SpwanType.SPECIFIC);
        SetItemType(ItemType.SALT);
        SetDescription("For a bit of flavour");
        SetQuanity((i == null ? 1 : (int)i));
        SetIsNew(true);
    }
}