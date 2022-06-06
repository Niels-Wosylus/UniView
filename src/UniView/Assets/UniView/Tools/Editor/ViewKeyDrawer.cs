using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UniView.Exposure;

namespace UniView.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ViewKeyAttribute))]
    public class ViewKeyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true) + 50;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
    
            var choices = GetChoices(property);
            var choiceIndex = GetChoiceIndex(property, choices);
            var popupPos = position.ShrinkBy(new RectOffset(0, 0, 40, 0));
            choiceIndex = EditorGUI.Popup(popupPos, label.text, choiceIndex, choices);
            property.stringValue = choices[choiceIndex];
    
            var boxPos = position.ShrinkBy(new RectOffset(0, 0, 0, 30));
            //EditorGUI.HelpBox(boxPos, "Displaying Health of Character from CharacterView", MessageType.Info);
        
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
            var consumer = ownerObject.GetComponent<IContentConsumer>();
            if (consumer == null)
            {
                Debug.LogWarning($"{nameof(ViewKeyAttribute)} should only appear on string fields belonging to a class implementing {nameof(IContentConsumer)}");
                return new[] { "" };
            }
                
            return new[] { "", "Choice 1", "Choice 2" };
        }
    }
}