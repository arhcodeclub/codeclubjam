using System.Collections.Generic;
using UnityEngine;

namespace Dialogue.ScriptableObjects
{
    using Data;
    using Enumerations;

    public class DSDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] [field: TextArea()] public string Speaker { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DSDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public DSDialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }
        [field: SerializeField] public float priority{get; set;}

        public void Initialize(string dialogueName, string text, string speaker, List<DSDialogueChoiceData> choices, DSDialogueType dialogueType, bool isStartingDialogue, float Priority)
        {
            DialogueName = dialogueName;
            Text = text;
            Speaker = speaker;
            Choices = choices;
            DialogueType = dialogueType;
            priority = Priority;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}