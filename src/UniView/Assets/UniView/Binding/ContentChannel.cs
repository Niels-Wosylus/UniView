using System;
using System.Collections.Generic;

namespace UniView.Binding
{
    public interface IContentChannel<in T>
    {
        string Key { get; }
        public Type OutputType { get; }
        void RegisterConsumer(IContentConsumer consumer);
        void Update(T content);
        void Clear();
    }
    
    public class ContentChannel<T, TOut> : IContentChannel<T>
    {
        public string Key { get; }
        public Type OutputType => typeof(TOut);
        
        private readonly List<IContentConsumer> _consumers = new List<IContentConsumer>(8);
        private readonly Func<T, TOut> _converter;

        public ContentChannel(string key, Func<T, TOut> converter)
        {
            Key = key;
            _converter = converter;
        }

        public void RegisterConsumer(IContentConsumer consumer)
        {
            _consumers.Add(consumer);
        }

        public void Update(T content)
        {
            var converted = _converter.Invoke(content);
            foreach (var consumer in _consumers)
            {
                consumer.Consume(converted);
            }
        }

        public void Clear()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Clear();
            }
        }
    }
}