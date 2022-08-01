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
                
                var choiceStrings = choices.Select(x => x.ToString()).ToArray();
                
                choiceIndex = EditorGUI.Popup(position, label.text, choiceIndex, choiceStrings);
                var key = choices[choiceIndex];
                key.AssignToProperty(property);
            }

            EditorGUI.EndProperty();
        }
        
        private static int GetChoiceIndex(SerializedProperty property, ViewKey[] choices)
        {
            var owner = property.serializedObject;
            var ownerObject = owner.targetObject;
            if (ownerObject is not ViewElementBase consumer)
                return 0;

            if (choices.Length == 0)
                return 0;

            var key = consumer.ViewKey;
            
            for (int i = 0; i < choices.Length; i++)
            {
                if (choices[i].Equals(key))
                    return i;
            }

            return choices.Length - 1;
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

            var all = (from source in sources
                let keys = source.GetAvailableKeysFor(consumer)
                from key in keys
                select new ViewKey(source, key)).ToList();
            
            all.Add(new ViewKey(null, null));
            return all.ToArray();
        }
    }
}