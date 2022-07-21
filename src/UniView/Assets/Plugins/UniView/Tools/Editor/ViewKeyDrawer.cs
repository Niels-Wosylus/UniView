using System;
using System.Collections.Generic;
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
    
            var choices = GetAvailableKeys(property);

            if (choices.Length > 0)
            {
                var choiceIndex = GetChoiceIndex(property, choices);
                var choiceStrings = choices.Select(x => $"{x.Source.name} :: {x.Key}").ToArray();
                choiceIndex = EditorGUI.Popup(position, label.text, choiceIndex, choiceStrings);
                var key = choices[choiceIndex];
                
                var owner = property.serializedObject;
                var ownerObject = owner.targetObject;
                if (ownerObject is not ViewElementBase consumer)
                    return;

                consumer.Key = key;
                consumer.OnValidate();
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

        private static int GetChoiceIndex(SerializedProperty property, ViewKey[] choices)
        {
            var owner = property.serializedObject;
            var ownerObject = owner.targetObject;
            if (ownerObject is not ViewElementBase consumer)
                return 0;

            var key = consumer.Key;
            
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i].Equals(key))
                    return i;
            }

            return 0;
        }

        private static ViewKey[] GetAvailableKeys(SerializedProperty property)
        {
            var owner = property.serializedObject;
            var ownerObject = owner.targetObject;
            if (ownerObject is not ViewElementBase consumer)
                return Array.Empty<ViewKey>();

            var transform = consumer.transform;
            var sources = new List<ViewBase>();
            while (transform != null)                            
            {                                                 
                var source = transform.GetComponent<ViewBase>();
                if (source != null && source != consumer) 
                    sources.Add(source);
                
                transform = transform.parent;                       
            }

            return (from source in sources
                let keys = source.GetAvailableKeysFor(consumer)
                from key in keys
                select new ViewKey(source, key)).ToArray();
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