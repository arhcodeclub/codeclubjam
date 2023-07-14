using Dialogue;
using Dialogue.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string requiredItem = "";
    public DSDialogue dialogueWithItem = null;
    public DSDialogue dialogueWithoutItem = null;

    public void startTalking()
    {
        if (inventorymanager.instance.checkItem(requiredItem))
        {
            dialogueWithItem.StartDialogue();
        }else
        {
            dialogueWithoutItem.StartDialogue();
        }
    }


}
