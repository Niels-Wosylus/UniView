using System;
using System.Collections.Generic;

namespace UniView.Exposure
{
    public interface IConsumerRegistry
    {
        void Register(IContentConsumer consumer, string key);
    }
    
    public interface IContentExposer<out TInput>
    {
        IContentExposure Expose<TExposed>(string key, Func<TInput, TExposed> function);
    }
    
    public interface IConsumerManager<T> : IDisposable, IContentExposer<T>, IConsumerRegistry
    {
        void Display(T content);
        void Clear();
    }
    
    public class ConsumerManager<T> : IConsumerManager<T>, IContentProvider<T>
    {
        private readonly Dictionary<string, IContentChannel> _channels = new Dictionary<string, IContentChannel>();
        private T _content;

        public IContentExposure Expose<TExposed>(string key, Func<T, TExposed> function)
        {
            if (_channels.ContainsKey(key))
                throw new Exception($"Key {key} is already being used to expose content");

            var channel = new ContentChannel<T, TExposed>(this, function);
            _channels.Add(key, channel);
            return channel;
        }

        public void Register(IContentConsumer consumer, string key)
        {
            if (!_channels.ContainsKey(key))
                throw new Exception($"Cannot register consumer, key {key} is not exposed");

            var channel = _channels[key];
            channel.RegisterConsumer(consumer);
        }

        public void Display(T content)
        {
            _content = content;
            foreach (var channel in _channels.Values)
            {
                channel.RefreshConsumers();
            }
        }

        public void Clear()
        {
            _content = default;
            foreach (var channel in _channels.Values)
            {
                channel.ClearConsumers();
            }
        }

        public T GetContent()
        {
            return _content;
        }
        
        public void Dispose()
        {
            foreach (var channel in _channels.Values)
            {
                channel.Dispose();
            }
        }
    }

    public interface IContentProvider<out T>
    {
        T GetContent();
    }
}