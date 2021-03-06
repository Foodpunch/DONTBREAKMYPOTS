﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChestInventory : Inventory {

    Bag bag;
    Inventory _inventory;
    public override void Start()
    {
        bag = GameObject.FindWithTag("Bag").GetComponent<Bag>();
        _inventory = bag.inv.GetComponent<Inventory>();
        _canvas = GameObject.FindWithTag("Canvas");
        DragAndDropTransform = DragAndDropIcon.GetComponent<RectTransform>();
        itemDatabase = GameObject.FindWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        int slotAmount = 0;
        x = xOffset;
        y = yOffset;
        for (int i = 1; i < rows + 1; i++)
        {
            for (int k = 1; k < columns + 1; k++)
            {
                GameObject slot = (GameObject)Instantiate(slots);
                slot.transform.SetParent(this.gameObject.transform, false);
                slot.GetComponent<SlotScript>().slotNum = slotAmount;
                slotsList.Add(slot);
                itemsList.Add(new Item());
                //slot.transform.parent = this.gameObject.transform;
                slot.name = "Slot" + i + "." + k;
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + xDistance / 2;
                if (k == columns)
                {
                    x = xOffset;
                    y = y - yDistance / 2;
                }
                slotAmount++;
            }
        }
        transform.gameObject.SetActive(false);
    }
    public void HideChest()
    {
        transform.gameObject.SetActive(false);
        _inventory.activeChest.itemsList = itemsList;
        bag.CloseBag();
        _inventory.activeChest = null;
        _inventory.activeChestInv = null;
    }
    
}
