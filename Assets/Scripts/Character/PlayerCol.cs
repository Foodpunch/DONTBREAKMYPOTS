using UnityEngine;
using System.Collections;

public class PlayerCol : MonoBehaviour {
    private Inventory inventoryScript;
	// Use this for initialization
	void Start () {
        inventoryScript = GameObject.FindGameObjectWithTag("Inventory").GetComponent<Inventory>();
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
