using System;
using System.Collections.Generic;
using UniView.Binding.Content;
using UniView.Binding.Signals;

namespace UniView.Binding
{
    public interface IViewBinder<T> : ISetup<T>, 
        IDisplay<T>, IContentConsumerRegistry, IContentSource
    {
        
    }
    
    public class ViewBinder<T> : IViewBinder<T>
    {
        private readonly IContentBroadcaster<T> _contentBroadcaster = new ContentBroadcaster<T>();
        private readonly ISignalBroadcaster _signalBroadcaster;
        private readonly ISignalReceiver _signalReceiver;
        
        public IContentChannelSetup<T> Content<TExposed>(string key, Func<T, TExposed> function)
        {
            return _contentBroadcaster.SetupContent(key, function);
        }

        public void Register(IContentConsumer consumer, string key)
        {
            _contentBroadcaster.Register(consumer, key);
        }

        public bool KeyIsAvailable(string key, IContentConsumer consumer)
        {
            return _contentBroadcaster.KeyIsAvailable(key, consumer);
        }

        public IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer)
        {
            return _contentBroadcaster.GetAvailableKeysFor(consumer);
        }

        public void Display(T content)
        {
            _contentBroadcaster.Display(content);
        }

        public void Clear()
        {
            _contentBroadcaster.Clear();
        }
    }
}