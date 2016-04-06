using UnityEngine;
using System.Collections;

public class Item {
    public string itemName;
    public string itemFileName;
    public int itemID;
    public string itemDesc;
    public Sprite itemIcon;
    public GameObject itemModel;
    public int itemATK;
    public int itemDEF;
    public int itemValue;
    public int itemAmount;
    public int maxStack;

    public enum ItemType
    {
        Weapon,
        Consumable,
        Pet,
        Placeable
    }

    public ItemType itemType;

    public Item(string fileName, string name, int id, string desc, int atk, int def, int value, int amount, ItemType type, int maximumStack)
    {
        itemName = name;
        itemID = id;
        itemDesc = desc;
        itemATK = atk;
        itemDEF = def;
        itemValue = value;
        itemAmount = amount;
        itemType = type;
        maxStack = maximumStack;
        itemIcon = Resources.Load<Sprite>("" + fileName);
    }

    public Item()
    {

    }
	
}
