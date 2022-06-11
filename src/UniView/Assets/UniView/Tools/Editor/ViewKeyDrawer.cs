﻿using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace UniView.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ViewKeyAttribute))]
    public class ViewKeyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
    
            var choices = GetChoices(property);

            if (choices.Length > 0)
            {
                var choiceIndex = GetChoiceIndex(property, choices);
                choiceIndex = EditorGUI.Popup(position, label.text, choiceIndex, choices);
                property.stringValue = choices[choiceIndex];
            }

            EditorGUI.EndProperty();
        }

        private int GetChoiceIndex(SerializedProperty property, string[] choices)
        {
            var value = property.stringValue;
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i] == value)
                    return i;
            }

            return 0;
        }

        private static string[] GetChoices(SerializedProperty property)
        {
            var owner = property.serializedObject;
            var ownerObject = owner.targetObject;
            var consumer = ownerObject as ViewElementBase;
            if (consumer == null)
            {
                Debug.LogWarning($"{nameof(ViewKeyAttribute)} should only appear on string fields belonging to a class implementing {nameof(ViewElementBase)}");
                return new[] { "" };
            }

            var parent = consumer.Parent;
            return parent == null ? new[] { "" } : parent.GetAvailableKeysFor(consumer).ToArray();
        }
    }
}