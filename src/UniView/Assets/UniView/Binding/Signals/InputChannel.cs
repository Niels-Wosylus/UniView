using System;
using System.Collections.Generic;

namespace UniView.Binding.Signals
{
    public interface IInputChannel
    {
        Type SignalType { get; }
    }
    
    public interface IInputChannel<T> : IInputChannel
    {
        void Receive(T signal);
        void AddHandler(Action<T> handler);
    }
    
    public class InputChannel<T> : IInputChannel<T>
    {
        private readonly List<Action<T>> _handlers = new List<Action<T>>(4);

        public Type SignalType => typeof(T);
        
        public void Receive(T signal)
        {
            foreach (var handler in _handlers)
            {
                handler.Invoke(signal);
            }
        }

        public void AddHandler(Action<T> handler)
        {
            _handlers.Add(handler);
        }
    }
}