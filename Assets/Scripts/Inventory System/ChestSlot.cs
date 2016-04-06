using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChestSlot : SlotScript, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler, IDragHandler
{
   
    ChestInventory chestInventory;
    public override void Start()
    {

        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        chestInventory = transform.parent.GetComponent<ChestInventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemAmount = gameObject.transform.GetChild(1).GetComponent<Text>();
    }
    public override void Update()
    {

        if (chestInventory.itemsList[slotNum].itemName != null)
        {
            itemAmount.enabled = false;
            itemImage.enabled = true;
            itemImage.sprite = inventory.itemsList[slotNum].itemIcon;
            if (chestInventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
            {
                itemAmount.enabled = true;
                itemAmount.text = "" + inventory.itemsList[slotNum].itemAmount;
            }
        }
        else
        {
            itemImage.enabled = false;
        }
    }
}
