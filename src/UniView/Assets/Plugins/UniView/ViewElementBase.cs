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
        [ViewKey]
        [SerializeField]
        private ViewKey _viewKey = default;

        public ViewKey ViewKey => _viewKey;
        public ViewBase Parent => _viewKey.Source;

        public abstract void Consume<TContent>(TContent content);
        public abstract bool CanConsume(Type contentType);
        public abstract void Clear();
        
        protected virtual void OnDispose() { }
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            if (string.IsNullOrEmpty(_viewKey.Key))
                return;
            
            registry.Register(this, _viewKey.Key);
        }

        protected virtual void OnDestroy()
        {
            OnDispose();
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            if (Parent != null)
                Parent.OnValidate();
        }
#endif
    }
}