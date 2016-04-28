using UnityEngine;
using System.Collections;

public class Bag : MonoBehaviour {
    public GameObject inv;
    bool isOpen;
	// Use this for initialization
	void Start () {
        isOpen = false;
	}
    public void ClickOnBag()
    {
        if (isOpen)
        {
            CloseBag();
        }
        else if (!isOpen)
        {
            OpenBag();
        }
    }

    public void OpenBag()
    {
        inv.SetActive(true);
        isOpen = true;
    }
    public void CloseBag()
    {
        inv.SetActive(false);
        isOpen = false;
    }
	
}
