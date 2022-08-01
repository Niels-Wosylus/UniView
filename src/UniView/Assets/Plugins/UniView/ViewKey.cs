using System;
using UnityEditor;
using UnityEngine;

namespace Wosylus.UniView
{
    [Serializable]
    public struct ViewKey
    {
        [SerializeField] private string _key;
        public string Key => _key;

        [SerializeField] private ViewBase _source;
        public ViewBase Source => _source;

        public ViewKey(ViewBase source, string key)
        {
            _source = source;
            _key = key;
        }

        public bool Equals(ViewKey other)
        {
            return Key == other.Key && Equals(Source, other.Source);
        }

        public override bool Equals(object obj)
        {
            return obj is ViewKey other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Key, Source);
        }

        public override string ToString()
        {
            return _source == null ? "--none--" : $"{_source.name} -> {_key}";
        }

#if UNITY_EDITOR
        public void AssignToProperty(SerializedProperty property)
        {
            var keyProp = property.FindPropertyRelative(nameof(_key));
            var sourceProp = property.FindPropertyRelative(nameof(_source));
            sourceProp.objectReferenceValue = _source;
            keyProp.stringValue = _key;
        }
#endif
    }
}