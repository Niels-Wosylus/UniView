using System;
using UnityEngine;
using UniView.Binding;
using UniView.Binding.Content;
using UniView.Tools;
using UniView.Utilities;

namespace UniView
{
    public abstract class ViewElementBase : MonoBehaviour, IContentConsumer
    {
        [Header("Source")]
        [ReadOnly]
        [SerializeField] private ViewBase _parent = default;
        public ViewBase Parent => _parent;
        
        [ViewKey]
        [SerializeField] private string _key;
        
        public abstract void Consume<TContent>(TContent content);
        public abstract bool CanConsume(Type contentType);
        public abstract void Clear();
        
        protected virtual void OnDispose() { }
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
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
            _parent = this.FindParent();
            if (_parent != null)
                _parent.OnValidate();
        }
#endif
    }
}