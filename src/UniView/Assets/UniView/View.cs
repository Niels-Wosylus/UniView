using System;
using System.Collections.Generic;
using UniView.Binding;

namespace UniView
{
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
        private static readonly IEqualityComparer<T> EqualityComparer = EqualityComparer<T>.Default;
        private ContentBroadcaster<T> _broadcaster;
        private T _displayedContent = default;
        private bool _isDisplayingContent;

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

            if (EqualityComparer.Equals(content, _displayedContent))
                return;
            
            if (content == null)
            {
                Clear();
                return;
            }

            OnClear(_displayedContent);
            _displayedContent = content;
            _isDisplayingContent = true;
            OnDisplay(content);
            _broadcaster.Display(content);
        }

        public void Refresh()
        {
            if (!_isDisplayingContent)
                return;
            
            _broadcaster.Display(_displayedContent);
        }

        public sealed override void Clear()
        {
            EnsureInitialization();
            
            if (!_isDisplayingContent)
                return;

            if(_displayedContent != null)
                OnClear(_displayedContent);
            
            _isDisplayingContent = false;
            _displayedContent = default;
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