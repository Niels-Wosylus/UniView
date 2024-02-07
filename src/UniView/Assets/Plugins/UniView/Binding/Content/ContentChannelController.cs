using System;
using UnityEngine;

namespace Wosylus.UniView.Binding.Content
{
    public interface IContentChannelController<T>
    {
        void Init(IContentChannel<T> channel, MonoBehaviour context);
        void OnInputChanged(T content);
        void OnClear();
        void Dispose();
    }
    
    public class ContentChannelController<T> : IContentChannelController<T>
    {
        private IContentChannel<T> _channel;
        private MonoBehaviour _context;

        public void Init(IContentChannel<T> channel, MonoBehaviour context)
        {
            _channel = channel;
            _context = context;
        }

        public void OnInputChanged(T content)
        {
            _channel.Update(content);
        }

        public void OnClear()
        {
            _channel.Clear();
        }

        public void Dispose()
        {
        }
    }

    public class RefreshContinuously<T> : IContentChannelController<T>, IGlobalUpdateCallback
    {
        private MonoBehaviour _context;
        private IContentChannel<T> _channel;
        private IDisposable _subscription;
        private T _content;
        private bool _isCleared;
        
        public void Init(IContentChannel<T> channel, MonoBehaviour context)
        {
            _channel = channel;
            _context = context;
            _subscription = GlobalUpdate.Register(this, _context);
        }

        public void OnInputChanged(T content)
        {
            _content = content;
            _isCleared = false;
        }

        public void OnClear()
        {
            _channel.Clear();
            _isCleared = true;
        }

        public void Dispose()
        {
            _subscription.Dispose();
        }

        public void Execute()
        {
            if(!_isCleared)
                _channel.Update(_content);
        }
    }
}