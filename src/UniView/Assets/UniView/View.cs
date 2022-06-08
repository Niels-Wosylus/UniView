using System;
using System.Collections.Generic;
using UniView.Binding;

namespace UniView
{
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
        private static readonly IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        public T DisplayedContent { get; private set; } = default;
        public bool IsDisplayingContent { get; private set; }
        private ContentBroadcaster<T> _broadcaster;

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
            _broadcaster.Display(content);
        }

        public void Refresh()
        {
            if (!IsDisplayingContent)
                return;
            
            _broadcaster.Display(DisplayedContent);
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
            _broadcaster.Clear();
        }
        
        protected abstract void Setup(ISetup<T> setup);
        
        protected virtual void OnInitialize() { }
        protected virtual void OnDisplay(T content) { }
        protected virtual void OnClear(T clearedContent) { }

        private void EnsureInitialization()
        {
            if (_broadcaster != null)
                return;
            
            _broadcaster = new ContentBroadcaster<T>();
            Setup(_broadcaster);
            RegisterElementsIn(_broadcaster);
            OnInitialize();
        }
        
        #region EDITOR
        
        public override bool KeyIsAvailable(string key, IContentConsumer consumer)
        {
            EnsureBroadcaster();
            return _broadcaster.KeyIsAvailable(key, consumer);
        }

        public override IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer)
        {
            EnsureBroadcaster();
            return _broadcaster.GetAvailableKeysFor(consumer);
        }
        
        private void EnsureBroadcaster()
        {
            if (_broadcaster != null) return;
            _broadcaster = new ContentBroadcaster<T>();
            Setup(_broadcaster);
        }

        #endregion

    }
}