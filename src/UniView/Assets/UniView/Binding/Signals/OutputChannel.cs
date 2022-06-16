using System;
using System.Collections.Generic;

namespace UniView.Binding.Signals
{
    public interface IOutputChannel<in T>
    {
        string Key { get; }
        public Type OutputType { get; }
        void RegisterConsumer(ISignalConsumer consumer);
        void Send(T signal);
    }
    
    public class OutputChannel<T> : IOutputChannel<T>
    {
        public string Key { get; }
        public Type OutputType => typeof(T);
        
        private readonly List<ISignalConsumer> _consumers = new List<ISignalConsumer>(8);

        public OutputChannel(string key)
        {
            Key = key;
        }

        public void RegisterConsumer(ISignalConsumer consumer)
        {
            _consumers.Add(consumer);
        }

        public void Send(T signal)
        {
            foreach (var consumer in _consumers)
            {
                consumer.Consume(Key, signal);
            }
        }
    }
}