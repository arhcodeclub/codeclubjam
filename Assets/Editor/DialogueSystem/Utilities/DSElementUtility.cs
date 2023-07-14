using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEngine.Events;

namespace Dialogue.Utilities
{
    using Elements;
    using UnityEngine;

    public static class DSElementUtility
    {
        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Port CreatePort(this DSNode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }

        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static FloatField CreatePriorityField(float value = 0, string label = null, EventCallback<ChangeEvent<float>> onValueChanged = null)
        {
            FloatField floatField = new FloatField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                floatField.RegisterValueChangedCallback(onValueChanged);
            }

            return floatField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }

        public static ObjectField CreateAudioField(AudioClip obj, Type objType, string label = null, EventCallback<ChangeEvent<Object>> onValueChanged = null)
        {
            ObjectField field = new ObjectField();
            field.objectType = objType;
            field.value = obj;
            field.label = label;
            field.allowSceneObjects = false;

            if (onValueChanged != null)
            {
                field.RegisterValueChangedCallback<Object>(onValueChanged);
            }

            return field;
        }
    }
}