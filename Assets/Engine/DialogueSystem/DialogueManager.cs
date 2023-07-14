using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Dialogue.ScriptableObjects;
using Dialogue.Data;

namespace Dialogue
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager instance;
        public GameObject dialoguePanel;
        public TMP_Text speakerNameText;
        public TMP_Text targetText;
        public DSDialogueSO currentDialogue;
        public GameObject currentSpeakerObject;
        public static bool inDialogue;

        public TMP_Text[] Choises;

        public float standardTextSpeed = 10;
        public float punctuationTextSpeed = 5;

        [Header("EffectStats")]
        public float shakeAmount = .1f;
        public float WaveStrength = 10f;
        public float WaveSpeed = 10f;

        public Gradient rainbowGradient;
        public float rainbowSpeed;
        public float RainbowPositionEffect;

        public bool pauseTalking = false;

        private List<TextEffectMarker> effectMarkers = new List<TextEffectMarker>();

        private UnityEvent OnFinishedTyping = new UnityEvent();

        private void Awake()
        {
            dialoguePanel.SetActive(false);

            instance = this;

        }

        private void Update()
        {
            UpdateEffects();
        }

        //Creates all choices or a close dialogue button and starts the typewriter function (Called once dialogue starts)
        private void StartReading()
        {
            if(currentDialogue.DialogueType == Enumerations.DSDialogueType.Event){
                GameObject eg = GameObject.Find(currentDialogue.Speaker);
                eg.SendMessage(currentDialogue.Text);

                NextDialogue(0);


                return;
            }

            //Set all choice objects inactive
            foreach(TMP_Text obj in Choises)
            {
                obj.transform.parent.gameObject.SetActive(false);
            }

            //create all choices
            int i = Choises.Length - 1;
            foreach(DSDialogueChoiceData choiceData in currentDialogue.Choices)
            {

                if (i <= 0)
                    break;
                
                Choises[i].text = choiceData.Text;
                Choises[i].transform.parent.gameObject.SetActive(true);

                i--;
            }

            //Set the name of the current speaker
            speakerNameText.text = currentDialogue.Speaker;

            ReadText(currentDialogue.Text);
        }

        //Go to the next Dialogue Line to read out
        public void NextDialogue(int index)
        {
            if (inDialogue)
            {
                StopAllCoroutines();
                bool inCommand = false;
                string textWIthoutCommands = "";
                foreach(char c in currentDialogue.Text)
                {
                    if(c == "<"[0])
                    {
                        inCommand = true;
                    }
                    else if (c == ">"[0])
                    {
                        inCommand = false;
                    }
                    else if (inCommand)
                    {
                        //DO NOTHING
                    }
                    else
                    {
                        textWIthoutCommands += c;
                    }
                }

                targetText.text = textWIthoutCommands;
                inDialogue = false;
                return;
            }

            
            //END Dialogue when nothing left to say...
            if (currentDialogue.Choices[index].NextDialogue == null)
            {
                dialoguePanel.SetActive(false);
           
                currentDialogue = null;
                currentSpeakerObject = null;
                OnFinishedTyping.Invoke();
                return;
            }


            currentDialogue = currentDialogue.Choices[index].NextDialogue;
            StartReading();
        }

        //Start a dialogue
        public void StartDialogue(DSDialogueSO dialogue, GameObject obj)
        {
            Debug.Log("Dialogue: Starting dialogue: " + dialogue.name);
            if(currentDialogue == null){
                dialoguePanel.SetActive(true);
                currentDialogue = dialogue;
                currentSpeakerObject = obj;
                StartReading();
                return;
            }

            if(currentDialogue.priority < dialogue.priority){
                dialoguePanel.SetActive(true);
                currentDialogue = dialogue;
                currentSpeakerObject = obj;
                StartReading();
            }else{
                OnFinishedTyping.AddListener(()=> {
                    StartDialogue(dialogue, obj);
                    OnFinishedTyping.RemoveAllListeners();
                });
            }
        }

        //Start the reading coroutine
        private void ReadText(string text)
        {
            targetText.text = "";

            StopAllCoroutines();
            StartCoroutine(Read(text));
        }

        //Reading coroutine
        IEnumerator Read(string text)
        {
            //Mark where effects are in the text
            SetEffectMarkers(text);

            inDialogue = true;

            bool inCommand = false;

            string finishedText = "";

            TextEffectMarker currentEffect = null;
            //add text to the textobject
            int indexInTextWithoutCommands = 0;
            for (int i = 0; i < text.Length; i++)
            {
                foreach(TextEffectMarker marker in effectMarkers)
                    {
                        if(marker.startIndex == indexInTextWithoutCommands)
                        {
                            currentEffect = marker;
                        }
                    }

                if (text[i] == "<"[0])
                {
                    inCommand = true;
                    continue;
                }
                else if (text[i] == ">"[0])
                {
                    inCommand = false;
                    continue;
                }

                if (inCommand)
                {
                    //DO NOTHING
                }
                //pausingCharacters are .  ,  !  ?  
                else if (text[i] != "."[0] && text[i] != ","[0] && text[i] != "!"[0] && text[i] != "?"[0])
                {
                    if(currentEffect != null && currentEffect.effect == TextEffect.Wait && indexInTextWithoutCommands == currentEffect.startIndex){
                        yield return new WaitForSeconds(currentEffect.waitTime / 1000);
                    }
                    else if (currentEffect != null && currentEffect.effect == TextEffect.Animate && indexInTextWithoutCommands == currentEffect.startIndex)
                    {
                        try
                        {
                            if(currentEffect.trigger != "")
                                currentSpeakerObject.GetComponent<Animator>().SetTrigger(currentEffect.trigger);
                        }
                        catch
                        {
                            Debug.LogError($"Dialogue: There was no animator found on {currentSpeakerObject.name}, The trigger [{currentEffect.trigger}] did not go off");
                        }
                    }

                    //Set Xtra Character
                    finishedText += text[i];
                    indexInTextWithoutCommands++;
                    targetText.text = finishedText;

                    //Wait
                    yield return new WaitForSeconds(1 / standardTextSpeed);
                }
                else
                {
                    //set Xtra Chara
                    finishedText += text[i];
                    indexInTextWithoutCommands++;

                    targetText.text = finishedText;

                    //Wait
                    yield return new WaitForSeconds(1 / punctuationTextSpeed);
                }

            }

            inCommand = false;

            inDialogue = false;

            yield return null;
        }

        //mark effect markers within text (<effect></effect>)
        private void SetEffectMarkers(string textWithCommands)
        {
            effectMarkers = new List<TextEffectMarker>();

            bool inCommandLocal = false;
            string commandText = "";
            int localIndexOffset = 0;
            int indexOffset = 0;
            int startIndex = 0;

            int j = 0;
            foreach(char i in textWithCommands)
            {
                if(i == "<"[0])
                {
                    localIndexOffset++;
                    inCommandLocal = true;
                    startIndex = j;
                }
                else if(i == ">"[0])
                {
                    localIndexOffset++;

                    inCommandLocal = false;
                    TextEffectMarker marker = new TextEffectMarker();

                    marker.startIndex = startIndex - indexOffset;

                    if (commandText == "none" || commandText[0] == "/"[0] || commandText == "n")
                    {
                        marker.effect = TextEffect.None;
                    }
                    else if (commandText == "wavy" || commandText == "w" || commandText == "wave")
                    {
                        marker.effect = TextEffect.Wavy;
                    }
                    else if (commandText == "shaky" || commandText == "s" || commandText == "shake")
                    {
                        marker.effect = TextEffect.Shaky;
                    }
                    else if (commandText == "rainbow" || commandText == "colors")
                    {
                        marker.effect = TextEffect.Rainbow;
                    }
                    else if(commandText.Remove(4) == "wait")
                    {
                        int waitTime = int.Parse(commandText.Remove(0, 5));
                        marker.effect = TextEffect.Wait;
                        marker.waitTime = waitTime;

                        TextEffectMarker m = new TextEffectMarker();
                        m.startIndex = startIndex - indexOffset + 1;

                        bool ididthething = false;
                        for(int index = effectMarkers.Count - 1; index >= 0; index--){
                            if(index < 0) return;

                            if(effectMarkers[index].effect != TextEffect.Wait){
                                m.effect = effectMarkers[index].effect;
                                ididthething = true;
                                break;
                            }
                        }

                        if(!ididthething){
                            m.effect = TextEffect.None;
                        }
                        effectMarkers.Add(m);

                    }
                    else if (commandText.Remove(4) == "anim")
                    {
                        string trigger = commandText.Remove(0, 5);


                        marker.effect = TextEffect.Animate;
                        marker.trigger = trigger;

                        TextEffectMarker m = new TextEffectMarker();
                        m.startIndex = startIndex - indexOffset + 1;

                        bool ididthething = false;
                        for (int index = effectMarkers.Count - 1; index >= 0; index--)
                        {
                            if (index < 0) return;

                            if (effectMarkers[index].effect != TextEffect.Animate)
                            {
                                m.effect = effectMarkers[index].effect;
                                ididthething = true;
                                break;
                            }
                        }

                        if (!ididthething)
                        {
                            m.effect = TextEffect.None;
                        }
                        effectMarkers.Add(m);

                    }
                    else
                    {
                        Debug.LogError("ERROR 404 COMMAND {" + commandText + "} NOT FOUND... ");
                        return;
                    }

                    effectMarkers.Add(marker);

                    commandText = "";

                    indexOffset += localIndexOffset;
                    localIndexOffset = 0;
                }
                else if (inCommandLocal)
                {
                    localIndexOffset++;
                    commandText += i;
                }

                j++;
            }
        }

        //Animate the text at the marker positions
        private void UpdateEffects()
        {
            targetText.ForceMeshUpdate();

            TMP_TextInfo textInfo = targetText.textInfo;
            TextEffect currentEffect = TextEffect.None;

            for(int i = 0; i < targetText.text.Length; i++)
            {
                foreach(TextEffectMarker marker in effectMarkers)
                {
                    if(marker.startIndex == i)
                    {
                        currentEffect = marker.effect;
                    }
                }

                switch (currentEffect)
                {
                    case TextEffect.None:
                        continue;
                    case TextEffect.Wavy:
                        WavyText(targetText.textInfo, i);
                        break;
                    case TextEffect.Shaky:
                        ShakyText(targetText.textInfo, i);
                        break;
                    case TextEffect.Wait:
                        //Check line 216
                        break;
                    case TextEffect.Animate:
                        //Check line 219
                        break;
                    case TextEffect.Rainbow:
                        RainbowGradient(targetText.textInfo, i);
                        break;

                }
            }

            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                //apply changes
                var meshInfo = textInfo.meshInfo[i];
                meshInfo.mesh.vertices = meshInfo.vertices;
                meshInfo.mesh.colors32 = meshInfo.colors32;
                targetText.UpdateGeometry(meshInfo.mesh, i);

            }
        }

        #region Effects

        public enum TextEffect
        {
            None,
            Shaky,
            Wavy,
            Wait,
            Animate,
            Rainbow
        }

        private void ShakyText(TMP_TextInfo textInfo, int i)
        {
            var charInfo = textInfo.characterInfo[i];

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(UnityEngine.Random.Range(-shakeAmount, shakeAmount), Random.Range(-shakeAmount, shakeAmount), 0);
            }
        }

        private void WavyText(TMP_TextInfo textInfo, int i)
        {
            var charInfo = textInfo.characterInfo[i];

            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;

            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                verts[charInfo.vertexIndex + j] = orig + new Vector3(0, Mathf.Sin(Time.time * WaveSpeed * 2f + orig.x * .01f) * WaveStrength, 0);
            }
        }

        private void RainbowGradient(TMP_TextInfo textInfo, int i)
        {
            var charInfo = textInfo.characterInfo[i];

            var colors = textInfo.meshInfo[charInfo.materialReferenceIndex].colors32;
            var verts = textInfo.meshInfo[charInfo.materialReferenceIndex].vertices;


            for (int j = 0; j < 4; j++)
            {
                var orig = verts[charInfo.vertexIndex + j];
                colors[charInfo.vertexIndex + j] = rainbowGradient.Evaluate((Mathf.Sin(Time.time * rainbowSpeed + orig.x * RainbowPositionEffect)/2 + 0.5f));
            }
        }

        #endregion
    }

    [System.Serializable]
    public class TextEffectMarker
    {
        public DialogueManager.TextEffect effect;
        public int startIndex;

        public int waitTime;

        public string trigger = "";
    }
}

