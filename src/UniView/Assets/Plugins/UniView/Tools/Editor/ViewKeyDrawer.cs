using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Wosylus.UniView.Tools.Editor
{
    [CustomPropertyDrawer(typeof(ViewKeyAttribute))]
    public class ViewKeyDrawer : PropertyDrawer
    {
        private static readonly string[] NoChoices = { "" };
    
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

        private static int GetChoiceIndex(SerializedProperty property, string[] choices)
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
                return NoChoices;
            }

            var parent = consumer.Parent;
            if (parent == null)
                return NoChoices;
            
            var choices = parent.GetAvailableKeysFor(consumer).ToArray();
            return choices.Length > 0 ? choices : NoChoices;
        }
    }
}