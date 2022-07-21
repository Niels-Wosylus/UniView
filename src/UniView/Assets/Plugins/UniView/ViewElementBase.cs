using System;
using UnityEngine;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Tools;
using Wosylus.UniView.Utilities;

namespace Wosylus.UniView
{
    public abstract class ViewElementBase : MonoBehaviour, IContentConsumer
    {
        [Header("Source")]
        [ReadOnly]
        [SerializeField] private ViewBase _parent = default;
        public ViewBase Parent => _parent;
        
        [ViewKey]
        [SerializeField] private string _key;

        [ViewKey]
        public ViewKey Key = default;

        public abstract void Consume<TContent>(TContent content);
        public abstract bool CanConsume(Type contentType);
        public abstract void Clear();
        
        protected virtual void OnDispose() { }
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            if (string.IsNullOrEmpty(_key))
                return;
            
            registry.Register(this, _key);
        }

        protected virtual void OnDestroy()
        {
            OnDispose();
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            SetParent();
        }

        private void SetParent()
        {
            _parent = Key.Source;
            //_parent = this.FindParent();
            if (_parent != null)
                _parent.OnValidate();
        }
#endif
    }

    [Serializable]
    public struct ViewKey
    {
        public readonly string Key;
        public readonly ViewBase Source;

        public ViewKey(ViewBase source, string key)
        {
            Source = source;
            Key = key;
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
    }
}