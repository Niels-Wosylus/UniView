using System;
using System.Collections.Generic;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Binding.Signals;

namespace Wosylus.UniView.Binding
{
    public interface IViewBinder<T> : ISetup<T>, 
        IDisplay<T>, IContentConsumerRegistry, IContentSource, 
        ISignalSource, ISignalConsumer, IDisposable
    {
        
    }
    
    public class ViewBinder<T> : IViewBinder<T>
    {
        private readonly IContentBroadcaster<T> _contentBroadcaster = new ContentBroadcaster<T>();
        private readonly ISignalBroadcaster _signalBroadcaster = new SignalBroadcaster();
        private readonly ISignalReceiver _signalReceiver = new SignalReceiver();
        
        public IContentChannelSetup<T, TOut> Content<TOut>(string key, Func<T, TOut> function)
        {
            return _contentBroadcaster.SetupContent(key, function);
        }

        public void Input<TSignal>(string key, Action<TSignal> handler)
        {
            _signalReceiver.SetupInput(key, handler);
        }

        public ISignalSender<TSignal> Output<TSignal>(string key)
        {
            return _signalBroadcaster.SetupOutput<TSignal>(key);
        }

        public void Output<TSignal>(string key, out ISignalSender<TSignal> output)
        {
            output = _signalBroadcaster.SetupOutput<TSignal>(key);
        }

        public void Register(IContentConsumer consumer, string key)
        {
            _contentBroadcaster.Register(consumer, key);
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

        public void Consume<TSignal>(string key, TSignal signal)
        {
            _signalReceiver.Consume(key, signal);
        }

        public bool CanConsume(string key, Type signalType)
        {
            return _signalReceiver.CanConsume(key, signalType);
        }

        public void Dispose()
        {
            _contentBroadcaster?.Clear();
            _contentBroadcaster?.Dispose();
        }
    }
}