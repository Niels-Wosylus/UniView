using System;
using UnityEngine;
using UniView.Binding;
using UniView.Tools;

namespace UniView
{
    public abstract class ViewElementBase : MonoBehaviour, IContentConsumer
    {
        [SerializeField] private ViewBase _parent = default;
        public ViewBase Parent => _parent;
        
        [ViewKey]
        [SerializeField] private string _key;
        
        public abstract void Consume<TContent>(TContent content);
        public abstract bool CanConsume(Type contentType);
        public abstract void Clear();
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            registry.Register(this, _key);
        }

        private void OnValidate()
        {
            Debug.Log($"Validating {name}");
        }
    }
}