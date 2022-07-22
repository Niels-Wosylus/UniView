using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Binding.Content.Processors;
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

        [SerializeField, ReadOnly]
        private ViewContentProcessor[] _preprocessors;
        private IContentProcessorChain _processorChain;

        protected abstract string InspectorPrefix { get; }

        public void Consume<TContent>(TContent content)
        {
            EnsureProcessorChain();
            _processorChain.Process(content);
        }

        public bool CanConsume(Type contentType)
        {
            EnsureProcessorChain();
            return _processorChain.CanProcess(contentType);
        }
        
        public abstract void Clear();
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            if (string.IsNullOrEmpty(_source.Key))
                return;
            
            registry.Register(this, _source.Key);
        }

        protected abstract IContentProcessor BuildContentProcessorFinalStep();
        
        protected virtual void OnDestroy()
        {
            OnDispose();
        }
        
        protected virtual void OnDispose() { }

        private void EnsureProcessorChain()
        {
            #if !UNITY_EDITOR
            if (_processorChain != null)
                return;
            #else
            if (_processorChain != null && Application.isPlaying)
                return;
            #endif
            
            _preprocessors = GetComponents<ViewContentProcessor>();
            var preprocessors = _preprocessors.Cast<IContentProcessor>().ToList();
            preprocessors.Add(BuildContentProcessorFinalStep());
            _processorChain = new ContentProcessorChain(preprocessors);
        }

#if UNITY_EDITOR
        public virtual void OnValidate()
        {
            _preprocessors = GetComponents<ViewContentProcessor>();
            
            if (Parent == null)
                return;
            
            if (!string.IsNullOrEmpty(InspectorPrefix))
                name = $"[{InspectorPrefix}] {_source.Key}";

            Parent.OnValidate();
        }
#endif
    }
}