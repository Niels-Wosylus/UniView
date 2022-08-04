#nullable enable
using System;

namespace UniViewV3.Messaging
{
    public interface IProcessor
    {
        
    }
    
    public interface IProcessor<in T> : IProcessor
    {
        void Process(T? message);
    }
    
    public class Processor<T> : IProcessor<T?>
    {
        private readonly Action<T?> _callback;

        public Processor(Action<T?> callback)
        {
            _callback = callback;
        }

        public void Process(T? message)
        {
            _callback.Invoke(message);
        }
    }
}