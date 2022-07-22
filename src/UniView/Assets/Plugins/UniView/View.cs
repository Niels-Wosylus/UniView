using System;
using System.Collections.Generic;
using Wosylus.UniView.Binding;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Binding.Content.Processors;

namespace Wosylus.UniView
{
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
        private static readonly IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        public T DisplayedContent { get; private set; } = default;
        public bool IsDisplayingContent { get; private set; }
        private ViewBinder<T> _binder;

        public void Display(T content)
        {
            EnsureInitialization();

            if (EqualityComparer.Equals(content, DisplayedContent))
                return;
            
            if (content == null)
            {
                Clear();
                return;
            }
            
            OnClear(DisplayedContent);
            foreach (var extender in GetComponents<ViewExtender>())
            {
                extender.OnDisplay(content);
            }
            DisplayedContent = content;
            IsDisplayingContent = true;
            OnDisplay(content);
            _binder.Display(content);
        }

        public sealed override void Clear()
        {
            EnsureInitialization();
            
            if (!IsDisplayingContent)
                return;

            if (DisplayedContent != null)
            {
                foreach (var extender in GetComponents<ViewExtender>())
                {
                    extender.OnClear(DisplayedContent);
                }
                OnClear(DisplayedContent);
            }

            IsDisplayingContent = false;
            DisplayedContent = default;
            _binder.Clear();
        }
        
        protected abstract void Setup(ISetup<T> setup);
        
        protected virtual void OnInitialize() { }
        protected virtual void OnDisplay(T content) { }
        protected virtual void OnClear(T clearedContent) { }

        private void EnsureInitialization()
        {
            if (_binder != null)
                return;
            
            _binder = new ViewBinder<T>();
            Setup(_binder);
            RegisterElementsIn(_binder);
            OnInitialize();
        }
        
        #region EDITOR
        public override IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer)
        {
            EnsureBinder();
            return _binder.GetAvailableKeysFor(consumer);
        }
        
        private void EnsureBinder()
        {
            if (_binder != null) return;
            _binder = new ViewBinder<T>();
            Setup(_binder);
        }
        #endregion
        
        protected override IContentProcessor BuildContentProcessorFinalStep() => new FinalStep(this);
        
        private class FinalStep : IContentProcessor
        {
            private readonly View<T> _owner;

            public FinalStep(View<T> owner)
            {
                _owner = owner;
            }

            public Type GetOutputType(Type inputType) => null;
            
            public bool CanProcess(Type inputType)
            {
                return typeof(T).IsAssignableFrom(inputType);
            }

            public void Process<TIn>(TIn content, IContentProcess process)
            {
                switch (content)
                {
                    case T match:
                        _owner.Display(match);
                        break;
                }
                
                process.EndedWith(this);
            }
        }
    }
}