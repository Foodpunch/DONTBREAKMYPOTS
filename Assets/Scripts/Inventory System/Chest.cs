using UnityEngine;
using System.Collections;

public class Chest : MonoBehaviour {
    Animator ChestAnimation;
    bool open = false;
    bool distanceChecker;
    PlayerCol playerCol;
    public float minRange;
    bool inRange = false;
    ChestInventory chestInv;
    public GameObject chestContents;
    GameObject _canvas;
    Inventory _inventory;
	// Use this for initialization
	void Start () {
        ChestAnimation = GetComponent<Animator>();
        playerCol = GameObject.FindWithTag("Player").GetComponent<PlayerCol>();
        _inventory = GameObject.FindWithTag("Inventory").GetComponent<Inventory>();
        //_canvas = GameObject.FindWithTag("Canvas");
        //chestInv = chestContents.GetComponent<ChestInventory>();
    }

    public void ShowChestContents()
    {
        //_inventory.activeChest
    }
    public void HideChestContents()
    {
        
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
	
	}
}
