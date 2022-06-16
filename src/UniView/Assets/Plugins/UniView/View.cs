using System;
using System.Collections.Generic;
using Wosylus.UniView.Binding;
using Wosylus.UniView.Binding.Content;

namespace Wosylus.UniView
{
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
        private static readonly IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        public T DisplayedContent { get; private set; } = default;
        public bool IsDisplayingContent { get; private set; }
        private ViewBinder<T> _binder;

        public override void Consume<TContent>(TContent content)
        {
            if(content is T match)
                Display(match);
        }
        
        public override bool CanConsume(Type contentType)
        {
            return typeof(T).IsAssignableFrom(contentType);
        }

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
            DisplayedContent = content;
            IsDisplayingContent = true;
            OnDisplay(content);
            _binder.Display(content);
        }

        public void Refresh()
        {
            if (!IsDisplayingContent)
                return;
            
            _binder.Display(DisplayedContent);
        }

        public sealed override void Clear()
        {
            EnsureInitialization();
            
            if (!IsDisplayingContent)
                return;

            if(DisplayedContent != null)
                OnClear(DisplayedContent);
            
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
        
        public override bool KeyIsAvailable(string key, IContentConsumer consumer)
        {
            EnsureBroadcaster();
            return _binder.KeyIsAvailable(key, consumer);
        }

        public override IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer)
        {
            EnsureBroadcaster();
            return _binder.GetAvailableKeysFor(consumer);
        }
        
        private void EnsureBroadcaster()
        {
            if (_binder != null) return;
            _binder = new ViewBinder<T>();
            Setup(_binder);
        }

        #endregion

    }

    public abstract class View<T, T1> : View<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                
                case T1 convertible:
                    var converted = Convert(convertible);
                    Display(converted);
                    break;
            }
        }
        
        public override bool CanConsume(Type contentType)
        {
            return typeof(T).IsAssignableFrom(contentType)
                || typeof(T1).IsAssignableFrom(contentType);
        }

        protected abstract T Convert(T1 content);
    }
    
    public abstract class View<T, T1, T2> : View<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                
                case T1 convertible1:
                    var converted1 = Convert(convertible1);
                    Display(converted1);
                    break;
                
                case T2 convertible2:
                    var converted2 = Convert(convertible2);
                    Display(converted2);
                    break;
            }
        }
        
        public override bool CanConsume(Type contentType)
        {
            return typeof(T).IsAssignableFrom(contentType)
                   || typeof(T1).IsAssignableFrom(contentType)
                   || typeof(T2).IsAssignableFrom(contentType);
        }

        protected abstract T Convert(T1 content);
        protected abstract T Convert(T2 content);
    }
}