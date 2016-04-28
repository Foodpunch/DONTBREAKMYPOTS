using UnityEngine;
using System.Collections;

public class PlayerCol : MonoBehaviour {
    private Inventory inventoryScript;
    private Bag bag;
	// Use this for initialization
	void Start () {
        bag = GameObject.FindGameObjectWithTag("Bag").GetComponent<Bag>();
        inventoryScript = bag.inv.GetComponent<Inventory>();
	}
	public void OnTriggerEnter2D(Collider2D other)
    {
        ItemPickup itemPickup = other.GetComponent<ItemPickup>();
        if(itemPickup != null)
        {
            inventoryScript.AddItem(itemPickup.itemId);
            Destroy(itemPickup.gameObject);
        }
    }
}
