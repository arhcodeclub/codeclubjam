using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Dialogue.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class DSEventNode : DSNode
    {
        public override void Initialize(string nodeName, DSGraphView dsGraphView, Vector2 position)
        {
            base.Initialize(nodeName, dsGraphView, position);

            Text = "Event name...";

            DialogueType = DSDialogueType.Event;

            DSChoiceSaveData choiceData = new DSChoiceSaveData()
            {
                Text = "Next"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            DrawBase();

            if(hasBuild){
                return;
            }


            /* OUTPUT CONTAINER */

            foreach (DSChoiceSaveData choice in Choices)
            {
                Port choicePort = this.CreatePort(choice.Text);

                choicePort.userData = choice;

                outputContainer.Add(choicePort);
            }
            

            RefreshExpandedState();

            hasBuild = true;

        }


        private void DrawBase(){
            /* TITLE CONTAINER */

                TextField dialogueNameTextField = DSElementUtility.CreateTextField(DialogueName, null, callback =>
                {
                    TextField target = (TextField) callback.target;

                    target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                    if (string.IsNullOrEmpty(target.value))
                    {
                        if (!string.IsNullOrEmpty(DialogueName))
                        {
                            ++graphView.NameErrorsAmount;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(DialogueName))
                        {
                            --graphView.NameErrorsAmount;
                        }
                    }

                    if (Group == null)
                    {
                        graphView.RemoveUngroupedNode(this);

                        DialogueName = target.value;

                        graphView.AddUngroupedNode(this);

                        return;
                    }

                    DSGroup currentGroup = Group;

                    graphView.RemoveGroupedNode(this, Group);

                    DialogueName = target.value;

                    graphView.AddGroupedNode(this, currentGroup);
                });

                dialogueNameTextField.AddClasses(
                    "ds-node__text-field",
                    "ds-node__text-field__hidden",
                    "ds-node__filename-text-field"
                );

                titleContainer.Insert(0, dialogueNameTextField);
                
                /* EXTENSION CONTAINER */

                VisualElement customDataContainer = new VisualElement();

                customDataContainer.AddToClassList("ds-node__custom-data-container");

                textFoldout = DSElementUtility.CreateFoldout("Event");


                TextField nameTextField = DSElementUtility.CreateTextArea(Speaker, "Gameobject Name", callback => Speaker = callback.newValue);
                TextField textTextField = DSElementUtility.CreateTextArea(Text, "Event Name", callback => Text = callback.newValue);

                storedSpeakeTextField = nameTextField;


                textFoldout.Add(nameTextField);
                textFoldout.Add(textTextField);

                customDataContainer.Add(textFoldout);

                extensionContainer.Add(customDataContainer);

                /* INPUT CONTAINER */

                Port inputPort = this.CreatePort("Dialogue Connection", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

                inputContainer.Add(inputPort);
        }
    }

}
