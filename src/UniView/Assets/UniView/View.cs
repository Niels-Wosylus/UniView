using System;
using System.Collections.Generic;
using UniView.Binding;

namespace UniView
{
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
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
            throw new NotImplementedException();
        }
        
        public sealed override void Clear()
        {
            throw new NotImplementedException();
        }
        
        protected abstract void Setup(ISetup<T> setup);

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