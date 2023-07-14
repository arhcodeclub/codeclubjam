using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Build;
using UnityEngine;

public class inventorymanager : MonoBehaviour
{
    public static inventorymanager instance;
    public itemslot[] slots;

    private void Awake()
    {
        instance = this;
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
            if (slot.itemInSlot.itemName == itemName)
            {
                Destroy(slot.itemInSlot);
                slot.itemInSlot = null;
            }
        }
    }
    public void removedonut()
    {
        RemoveItem("normaldonut");
    }


}
