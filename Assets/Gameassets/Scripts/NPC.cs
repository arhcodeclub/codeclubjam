using Dialogue;
using Dialogue.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public DSDialogue dialogueWithoutItem = null;
    [Header("item 1")]
    public string requiredItem = "";
    public DSDialogue dialogueWithItem = null;
    [Header("item 2")]
    public string requiredItem2 = "";
    public DSDialogue dialogueWithItem2 = null;
    [Header("item 3")]
    public string requiredItem3 = "";
    public DSDialogue dialogueWithItem3 = null;


    public void startTalking()
    {
        if (inventorymanager.instance.checkItem(requiredItem))
        {
            dialogueWithItem.StartDialogue();
            return;
        }else
        {
            if (inventorymanager.instance.checkItem(requiredItem2))
            {
                dialogueWithItem2.StartDialogue();
                return;

            }
            else if (inventorymanager.instance.checkItem(requiredItem3))
            {
                dialogueWithItem3.StartDialogue();
                return;

            }
            else dialogueWithoutItem.StartDialogue();
        }
    }


}
