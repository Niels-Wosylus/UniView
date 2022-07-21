using System;
using UnityEngine;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Tools;

namespace Wosylus.UniView
{
    public abstract class ViewElementBase : MonoBehaviour, IContentConsumer
    {
        [ViewKey]
        [SerializeField]
        private ViewKey _source = default;

        public ViewKey ViewKey => _source;
        public ViewBase Parent => _source.Source;

        protected abstract string InspectorPrefix { get; }
        
        public abstract void Consume<TContent>(TContent content);
        public abstract bool CanConsume(Type contentType);
        public abstract void Clear();
        
        protected virtual void OnDispose() { }
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            if (string.IsNullOrEmpty(_source.Key))
                return;
            
            registry.Register(this, _source.Key);
        }

        protected virtual void OnDestroy()
        {
            OnDispose();
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            if (Parent == null)
                return;
            
            if (!string.IsNullOrEmpty(InspectorPrefix))
                name = $"[{InspectorPrefix}] {_source.Key}";

            Parent.OnValidate();
        }
#endif
    }
}