using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlot : SlotScript, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
    Bag bag;
    ChestInventory chestInventory;
    public override void Start()
    {
        bag = GameObject.FindWithTag("Bag").GetComponent<Bag>();
        inventory = bag.inv.GetComponent<Inventory>();
        chestInventory = transform.parent.GetComponent<ChestInventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemAmount = gameObject.transform.GetChild(1).GetComponent<Text>();
    }
    public override void Update()
    {
        //base.Update();
        if (chestInventory.itemsList[slotNum].itemName != null)
        {
            itemAmount.enabled = false;
            itemImage.enabled = true;
            itemImage.sprite = chestInventory.itemsList[slotNum].itemIcon;
            if (chestInventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
            {
                itemAmount.enabled = true;
                itemAmount.text = "" + chestInventory.itemsList[slotNum].itemAmount;
            }
        }
        else
        {
            itemImage.enabled = false;
        }
    }
    public override void OnPointerDown(PointerEventData data)
    {
        if ((Time.time - doubleClickStart) <= 0.3f)
        {
            //if (inventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
            //{
            //    inventory.itemsList[slotNum].itemAmount--;
            //    if (inventory.itemsList[slotNum].itemAmount == 0)
            //    {
            //        inventory.itemsList[slotNum] = new Item();
            //        itemAmount.enabled = false;
            //        inventory.HideToolTip();
            //    }
            //}
            if(chestInventory.itemsList[slotNum].itemName != null)
            {
                inventory.AddItem(chestInventory.itemsList[slotNum].itemID);
                chestInventory.itemsList[slotNum] = new Item();
                if(chestInventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
                {
                    itemAmount.enabled = false;
                    chestInventory.HideToolTip();
                }
                itemAmount.enabled = false;
            }
            
            doubleClickStart = -1;
        }
        else
        {
            doubleClickStart = Time.time;
        }



        if (chestInventory.itemsList[slotNum].itemName == null && inventory.draggingItem)
        {
            chestInventory.itemsList[slotNum] = inventory.draggedItem;
            inventory.HideDraggedItem();
        }
        else if (chestInventory.itemsList[slotNum].itemName != null && inventory.draggingItem)
        {
            if (inventory.draggingFromInventory)
            {
                inventory.itemsList[inventory.draggedItemIndex] = chestInventory.itemsList[slotNum];
                chestInventory.itemsList[slotNum] = inventory.draggedItem;
                inventory.HideDraggedItem();
            }
            else
            {
                chestInventory.itemsList[inventory.draggedItemIndex] = chestInventory.itemsList[slotNum];
                chestInventory.itemsList[slotNum] = inventory.draggedItem;
                inventory.HideDraggedItem();
            }
            
        }
    }
    public override void OnPointerEnter(PointerEventData data)
    {
        if (chestInventory.itemsList[slotNum].itemName != null)
        {
            chestInventory.ShowToolTip(chestInventory.slotsList[slotNum].GetComponent<RectTransform>().localPosition, chestInventory.itemsList[slotNum]);
        }
    }
    public override void OnPointerExit(PointerEventData data)
    {
        if (chestInventory.itemsList[slotNum].itemName != null)
        {
            chestInventory.HideToolTip();
        }

    }
    public override void OnDrag(PointerEventData data)
    {
        if (chestInventory.itemsList[slotNum].itemName != null)
        {
            inventory.draggingFromInventory = false;
            inventory.ShowDraggedItem(chestInventory.itemsList[slotNum], slotNum);
            chestInventory.itemsList[slotNum] = new Item();
            itemAmount.enabled = false;
        }
    }
}
