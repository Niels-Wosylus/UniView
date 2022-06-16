using System;

namespace Wosylus.UniView.Binding.Content
{
    public interface IContentChannelController<T>
    {
        void Init(IContentChannel<T> channel);
        void OnContentChanged(T content);
        void OnClear();
        void Dispose();
    }
    
    public class ContentChannelController<T> : IContentChannelController<T>
    {
        private IContentChannel<T> _channel;

        public void Init(IContentChannel<T> channel)
        {
            _channel = channel;
        }

        public void OnContentChanged(T content)
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

    public class RefreshContinuously<T> : IContentChannelController<T>
    {
        private IContentChannel<T> _channel;
        private IDisposable _subscription;
        private T _content;
        private bool _isCleared;
        
        public void Init(IContentChannel<T> channel)
        {
            _channel = channel;
            _subscription = GlobalUpdate.Register(() =>
            {
                if(!_isCleared)
                    _channel.Update(_content);
            });
        }

        public void OnContentChanged(T content)
        {
            _content = content;
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
    }
}