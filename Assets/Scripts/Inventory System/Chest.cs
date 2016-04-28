using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Chest : MonoBehaviour {
    Animator ChestAnimation;

    public int rows;
    public int columns;
    bool open = false;
    bool distanceChecker;
    PlayerCol playerCol;
    public float minRange;
    bool inRange = false;
    ChestInventory chestInv;
    public GameObject chestContents;
    GameObject _canvas;
    Inventory _inventory;
    public int xOffset;
    public int x;
    public int y;
    public int xDistance;
    public int yDistance;

    public List<Item> itemsList = new List<Item>();
    //public List<GameObject> slotsList = new List<GameObject>();

    private Bag bag;
	// Use this for initialization
	void Start () {
        ChestAnimation = GetComponent<Animator>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<PlayerCol>();
        _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        bag = GameObject.FindWithTag("Bag").GetComponent<Bag>();
        chestContents = GameObject.FindWithTag("Chest");
        _canvas = GameObject.FindWithTag("Canvas");
        for (int i = 1; i < rows + 1; i++)
        {
            for (int k = 1; k < columns + 1; k++)
            {
                //GameObject slot = (GameObject)Instantiate(slots);
                //slot.transform.SetParent(this.gameObject.transform, false);
                //slot.GetComponent<SlotScript>().slotNum = slotAmount;
                //slotsList.Add(slot);
                itemsList.Add(new Item());
                //slot.transform.parent = this.gameObject.transform;
                //slot.name = "Slot" + i + "." + k;
                //slot.GetComponent<RectTransform>().localPosition = new Vector3(x, y, 0);
                x = x + xDistance / 2;
                if (k == columns)
                {
                    x = xOffset;
                    y = y - yDistance / 2;
                }
            }
        }
        //chestInv = chestContents.GetComponent<ChestInventory>();
    }

    public void ShowChestContents()
    {
        Vector2 chestPos = Camera.main.WorldToViewportPoint(transform.position);
        chestContents.GetComponent<RectTransform>().localPosition = new Vector3(chestPos.x + xOffset, chestPos.y, 0);
        chestContents.SetActive(true);
        bag.OpenBag();
        _inventory.activeChest = this;
        _inventory.activeChestInv = chestContents.GetComponent<ChestInventory>();
        _inventory.activeChestInv.itemsList = itemsList;
        //_inventory.activeChest
    }
    public void HideChestContents()
    {
        chestContents.SetActive(false);
        itemsList = _inventory.activeChestInv.itemsList;
        bag.CloseBag();
        _inventory.activeChest = null;
        _inventory.activeChestInv = null;
    }

    void OnMouseOver()
    {
        Vector2 offset = playerCol.transform.position - transform.position;
        float sqrLen = offset.sqrMagnitude;
        if(sqrLen < minRange * minRange)
        {
            if (!open)
            {
                ChestAnimation.SetInteger("_state", 1);
                if (Input.GetMouseButtonDown(0))
                {
                    open = true;
                    ChestAnimation.SetInteger("_state", 1);

                    ShowChestContents();
                    // Debug.Log("Fuck");
                    // ShowChestContents();
                }
            }
            else
            {
                
            }
        }
    }
	void OnMouseExit()
    {
        if (!open)
        {
            ChestAnimation.SetInteger("_state", 0);
        }
        else
        {
            open = false;
            //HideChestContents();
            ChestAnimation.SetInteger("_state", 0);
        }


    }
	// Update is called once per frame
	void Update () {
        if(_inventory.activeChest == this)
        {

            Vector2 chestPos = Camera.main.WorldToScreenPoint(transform.position);
            chestContents.transform.position = new Vector3(chestPos.x + xOffset, chestPos.y, 0);
        }
    }
}
