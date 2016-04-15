using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotScript : MonoBehaviour, IPointerDownHandler ,IPointerUpHandler , IPointerEnterHandler , IPointerExitHandler , IDragHandler {

    public Item item;
    public Image itemImage;
    public int slotNum;
    public Text itemAmount;

    public Inventory inventory;
    public float doubleClickStart = 0;
    // Use this for initialization
    public virtual void Start () {
        
        inventory = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
        itemImage = gameObject.transform.GetChild(0).GetComponent<Image>();
        itemAmount = gameObject.transform.GetChild(1).GetComponent<Text>();
    }
	
	// Update is called once per frame
	public virtual void Update () {
	    if(inventory.itemsList[slotNum].itemName != null)
        {
            itemAmount.enabled = false; 
            itemImage.enabled = true;
            itemImage.sprite = inventory.itemsList[slotNum].itemIcon;
            if(inventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
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
    public virtual void OnPointerDown(PointerEventData data)
    {
        if((Time.time - doubleClickStart) <= 0.3f)
        {
            if (inventory.itemsList[slotNum].itemType == Item.ItemType.Consumable)
            {
                inventory.itemsList[slotNum].itemAmount--;
                if (inventory.itemsList[slotNum].itemAmount == 0)
                {
                    inventory.itemsList[slotNum] = new Item();
                    itemAmount.enabled = false;
                    inventory.HideToolTip();
                }
            }
            doubleClickStart = -1;
        }
        else
        {
            doubleClickStart = Time.time;
        }


        if (inventory.itemsList[slotNum].itemName == null && inventory.draggingItem)
        {
            inventory.itemsList[slotNum] = inventory.draggedItem;
            inventory.HideDraggedItem();
        }
        else if (inventory.itemsList[slotNum].itemName != null && inventory.draggingItem && inventory.activeChestInv == null)
        {
            inventory.itemsList[inventory.draggedItemIndex] = inventory.itemsList[slotNum];
            inventory.itemsList[slotNum] = inventory.draggedItem;
            inventory.HideDraggedItem();
        }
        else if (inventory.itemsList[slotNum].itemName != null && inventory.draggingItem && inventory.activeChestInv != null)
        {
            if (!inventory.draggingFromInventory)
            {
                inventory.activeChestInv.itemsList[inventory.draggedItemIndex] = inventory.itemsList[slotNum];
                inventory.itemsList[slotNum] = inventory.draggedItem;
                inventory.HideDraggedItem();
            }
            else
            {
                inventory.itemsList[inventory.draggedItemIndex] = inventory.itemsList[slotNum];
                inventory.itemsList[slotNum] = inventory.draggedItem;
                inventory.HideDraggedItem();
            }
        }


    }
    public virtual void OnPointerUp(PointerEventData data)
    {
        //if (inventory.itemsList[slotNum].itemName == null && inventory.draggingItem)
        //{
        //    inventory.itemsList[slotNum] = inventory.draggedItem;
        //    inventory.HideDraggedItem();
        //}
        //else if (inventory.itemsList[slotNum].itemName != null && inventory.draggingItem)
        //{
        //    inventory.itemsList[inventory.draggedItemIndex] = inventory.itemsList[slotNum];
        //    inventory.itemsList[slotNum] = inventory.draggedItem;
        //    inventory.HideDraggedItem();
        //}
    }

    public virtual void OnPointerEnter(PointerEventData data)
    {
        if (inventory.itemsList[slotNum].itemName != null)
        {
            inventory.ShowToolTip(inventory.slotsList[slotNum].GetComponent<RectTransform>().localPosition, inventory.itemsList[slotNum]);
        }
    }
    public virtual void OnPointerExit(PointerEventData data)
    {
        if (inventory.itemsList[slotNum].itemName != null)
        {
            inventory.HideToolTip();
        }
            
    }
    public virtual void OnDrag(PointerEventData data)
    {
        if (inventory.itemsList[slotNum].itemName != null)
        {
            inventory.draggingFromInventory = true;
            inventory.ShowDraggedItem(inventory.itemsList[slotNum], slotNum);
            inventory.itemsList[slotNum] = new Item();
            itemAmount.enabled = false;
        }
        
    }
    
}
