using Dialogue;
using Dialogue.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public string requiredItem = "";
    DSDialogueSO dialogueWithItem = null;
    DSDialogueSO dialogueWithoutItem = null;

    public void startTalking()
    {
        if (inventorymanager.instance.checkItem(requiredItem))
        {
            DialogueManager.instance.StartDialogue(dialogueWithItem, gameObject);
        }else
        {
            DialogueManager.instance.StartDialogue(dialogueWithoutItem, gameObject);
        }
    }


}
