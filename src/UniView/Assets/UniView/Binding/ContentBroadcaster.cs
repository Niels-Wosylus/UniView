using System;
using System.Collections.Generic;

namespace UniView.Binding
{
    public interface IContentConsumerRegistry
    {
        void Register(IContentConsumer consumer, string key);
    }

    public interface IContentBroadcaster<T> : IDisplay<T>, ISetup<T>, IContentConsumerRegistry, IContentSource, IDisposable
    {
        void OverrideContentChannelController(string key, IContentChannelController<T> controller);
    }
    
    public class ContentBroadcaster<T> : IContentBroadcaster<T>
    {
        private readonly Dictionary<string, IContentChannel<T>> _channels 
            = new Dictionary<string, IContentChannel<T>>();
        
        private readonly Dictionary<string, IContentChannelController<T>> _controllers
            = new Dictionary<string, IContentChannelController<T>>();

        private readonly HashSet<IContentChannelController<T>> _activeControllers
            = new HashSet<IContentChannelController<T>>();

        public IContentChannelSetup<T> Content<TExposed>(string key, Func<T, TExposed> function)
        {
            if (_channels.ContainsKey(key))
                throw new Exception($"Key {key} is already being used to expose content");

            var channel = new ContentChannel<T, TExposed>(key, function);
            _channels.Add(key, channel);
            return new ContentChannelSetup<T>(channel, this);
        }

        public void OverrideContentChannelController(string key, IContentChannelController<T> controller)
        {
            _controllers[key] = controller;
        }

        public void Register(IContentConsumer consumer, string key)
        {
            if (!_channels.ContainsKey(key))
                throw new Exception($"Cannot register consumer, key {key} is not exposed");
            
            if(!_controllers.ContainsKey(key))
                _controllers.Add(key, new ContentChannelController<T>());

            var controller = _controllers[key];
            if (!_activeControllers.Contains(controller))
                _activeControllers.Add(controller);

            var channel = _channels[key];
            channel.RegisterConsumer(consumer);
            controller.Init(channel);
        }

        public void Display(T content)
        {
            foreach (var controller in _activeControllers)
            {
                controller.OnContentChanged(content);
            }
        }

        public void Clear()
        {
            foreach (var controller in _activeControllers)
            {
                controller.OnClear();
            }
        }

        public void Dispose()
        {
            foreach (var controller in _activeControllers)
            {
                controller.Dispose();
            }
        }

        public bool KeyIsAvailable(string key, IContentConsumer consumer)
        {
            if (!_channels.ContainsKey(key)) 
                return false;

            var channel = _channels[key];
            var type = channel.OutputType;
            return consumer.CanConsume(type);
        }

        public IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer)
        {
            foreach (var (key, channel) in _channels)
            {
                var type = channel.OutputType;
                if (consumer.CanConsume(type))
                    yield return key;
            }
        }
    }
}