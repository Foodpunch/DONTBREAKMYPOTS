using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemDatabase : MonoBehaviour {

    public List<Item> items = new List<Item>();
	// Use this for initialization
	void Start () {
        items.Add(new Item("nChest", "Regular Chest", 0000, "A Regular Chest", 999, 999, 40, 1, Item.ItemType.Placeable, 1));
        items.Add(new Item("rSlime", "Red Slime", 0001, "A Red Slime", 20, 10, 120, 1, Item.ItemType.Pet, 1));
        items.Add(new Item("pSlime", "Purple Slime", 0002, "A Purple Slime", 10, 20, 180, 1, Item.ItemType.Pet, 1));
        items.Add(new Item("mMana", "Medium Mana Potion", 0003, "Restores a medium amount of mana", 999, 999, 50, 1, Item.ItemType.Consumable, 99));
        items.Add(new Item("cGranny", "Cranky Grandma", 0004, "Why do you have an old woman in your bag????", 10, 20, 180, 1, Item.ItemType.Pet, 1));

    }
	
}
