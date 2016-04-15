using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Inventory : MonoBehaviour {

    public List<GameObject> slotsList = new List<GameObject>();
    public int rows;
    public int columns;
    public List<Item> itemsList = new List<Item>();
    public int toolTipXOffset;
    public ItemDatabase itemDatabase;
    public GameObject slots;
    public int xDistance;
    public int yDistance;
    public int xOffset;
    public int yOffset;
    public int x;
    public int y;
    public GameObject ToolTip;
    public GameObject DragAndDropIcon;
    public RectTransform DragAndDropTransform;
    public int DaDXOffset;
    public int DaDYOffset;
    public bool draggingItem;
    public Item draggedItem;
    public int draggedItemIndex;
    public GameObject _canvas;
    public bool draggingFromInventory;
    public ChestInventory activeChestInv;
    public Chest activeChest;
    // Use this for initialization
    public virtual void Start () {
        _canvas = GameObject.FindWithTag("Canvas");
        DragAndDropTransform = DragAndDropIcon.GetComponent<RectTransform>();
        itemDatabase = GameObject.FindWithTag("ItemDatabase").GetComponent<ItemDatabase>();
        int slotAmount = 0;
        x = xOffset;
        y = yOffset;
        for (int i = 1; i < rows + 1; i++)
        {
            for(int k = 1; k < columns + 1; k++)
            {
                GameObject slot = (GameObject)Instantiate(slots);
                slot.transform.SetParent(this.gameObject.transform, false);
                slot.GetComponent<SlotScript>().slotNum = slotAmount;
                slotsList.Add(slot);
                itemsList.Add(new Item());
                //slot.transform.parent = this.gameObject.transform;
                slot.name = "Slot"+ i + "." + k;
                slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + xDistance / 2;
                if(k == columns)
                {
                    x = xOffset;
                    y = y - yDistance / 2;
                }
                slotAmount++;
            }
        }
        transform.gameObject.SetActive(false);
    }
    public void CheckIfItemExists(int id, Item item)
    {
        for(int i = 0; i < itemsList.Count; i++)
        {
            if(itemsList[i].itemID == id)
            {
                item.itemAmount = item.itemAmount + 1;
                break;
            }
            else if(i == itemsList.Count - 1)
            {
                AddItemAtFreeSlot(item);
            }
        }
    }
    public void AddItem(int id)
    {
        for (int i = 0; i < itemDatabase.items.Count; i++)
        {
            if(itemDatabase.items[i].itemID == id)
            {
                Item item = itemDatabase.items[i];
                if (itemDatabase.items[i].itemType == Item.ItemType.Consumable)
                {
                    CheckIfItemExists(id, item);
                    break;
                }
                else
                {
                    
                    AddItemAtFreeSlot(item);
                    break;
                }
                
            }
        }
    }

    public void AddItemAtFreeSlot(Item item)
    {
        for (int i = 0; i < itemsList.Count; i++)
        {
            if (itemsList[i].itemName == null)
            {
                itemsList[i] = item;
                break;
            }
        }
    }

    public void ShowToolTip(Vector3 toolPos, Item item)
    {
        ToolTip.GetComponent<RectTransform>().localPosition = new Vector3(toolPos.x + toolTipXOffset, toolPos.y, toolPos.z);
        ToolTip.transform.GetChild(0).GetComponent<Text>().text = item.itemName;
        ToolTip.transform.GetChild(1).GetComponent<Text>().text = "ATK: " + item.itemATK.ToString();
        ToolTip.transform.GetChild(2).GetComponent<Text>().text = "DEF: " + item.itemDEF.ToString();
        ToolTip.transform.GetChild(3).GetComponent<Text>().text = "VALUE: " + item.itemValue.ToString();
        ToolTip.transform.GetChild(4).GetComponent<Text>().text = item.itemDesc;
        ToolTip.SetActive(true);
    }
    public void HideToolTip()
    {
        ToolTip.SetActive(false);
    }

    public void ShowDraggedItem(Item item, int slotNum)
    {
        draggedItemIndex = slotNum;
        HideToolTip();
        draggedItem = item;
        DragAndDropIcon.SetActive(true);
        
        DragAndDropIcon.GetComponent<Image>().sprite = item.itemIcon;
        draggingItem = true;
    }

    public void HideDraggedItem()
    {
        draggingItem = false;
        DragAndDropIcon.SetActive(false);
    }

    // Update is called once per frame
    void Update () {
        if (draggingItem)
        {
            Vector3 pos = (Input.mousePosition - _canvas.GetComponent<RectTransform>().localPosition);
            DragAndDropTransform.localPosition = new Vector3(pos.x, pos.y, pos.z);
        }
	}
}
