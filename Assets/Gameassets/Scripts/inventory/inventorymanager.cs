using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inventorymanager : MonoBehaviour
{
    public static inventorymanager instance;
    public itemslot[] slots;

    private void Awake()
    {
        instance = this;
        addmoney();
    }

    public void AddItem(GameObject itemToAdd)
    {
        foreach (var slot in slots)
        {
            if (slot.itemInSlot == null)
            {
                GameObject spawned = Instantiate(itemToAdd);
                spawned.GetComponent<item>().slot = slot;
                slot.itemInSlot = itemToAdd.GetComponent<item>();
                spawned.transform.SetParent(slot.transform, false);
                return;
            }
        }
    }
    public bool checkItem(string itemName)
    {
        foreach(var slot in slots) {
            if (slot.itemInSlot != null)
            {
                if (slot.itemInSlot.itemName == itemName)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void RemoveItem(string itemName)
    {
        foreach(var slot in slots)
        {
            if (slot.itemInSlot != null)
            {
                if (slot.itemInSlot.itemName == itemName)
                {
                    Destroy(slot.itemInSlot.gameObject);
                    slot.itemInSlot = null;
                }
            }
        }
    }
    public void removedonut()
    {
        RemoveItem("normaldonut");
    }
    public void removemoney()
    {
        RemoveItem("dollar");
    }
    public GameObject normaldonut;
    public void addnormaldonut()
    {
        AddItem(normaldonut);
    }
    public GameObject dollar;
    public void addmoney()
    {
        AddItem(dollar);
    }
    public GameObject wirecutter;
    public void addwirecutter()
    {
        AddItem(wirecutter);
    }


}
