using UnityEngine;
using Dialogue.ScriptableObjects;
using Dialogue;


public class DSDialogue : Interactable
    {
        /* Dialogue Scriptable Objects */
        [SerializeField] private DSDialogueContainerSO dialogueContainer;
        [SerializeField] private DSDialogueGroupSO dialogueGroup;
        public DSDialogueSO dialogue;

        /* Filters */
        [SerializeField] private bool groupedDialogues;
        [SerializeField] private bool startingDialoguesOnly = true;

        /* Indexes */
        [SerializeField] private int selectedDialogueGroupIndex;
        [SerializeField] private int selectedDialogueIndex;

        public void StartDialogue()
        {
            if (!gameObject.activeInHierarchy) { return;  }

            DialogueManager.instance.StartDialogue(dialogue, gameObject);
        }

        public override void Interact()
        {
            StartDialogue();
        }
    }
