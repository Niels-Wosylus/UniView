using System;

namespace UniView.Binding.Signals
{
    public interface ISignalConsumer
    {
        void Consume<TSignal>(string key, TSignal content);
        bool CanConsume(string key, Type signalType);
    }
}