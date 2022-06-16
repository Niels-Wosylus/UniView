using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniView.Binding.Signals
{
    public interface ISignalReceiver : ISignalConsumer
    {
        void SetupInput<T>(string key, Action<T> handler);
    }
    
    public class SignalReceiver : ISignalReceiver
    {
        private readonly Dictionary<string, IInputChannel> _channels = new Dictionary<string, IInputChannel>();

        public void Consume<TSignal>(string key, TSignal signal)
        {
            if (!_channels.ContainsKey(key))
                return;

            var channel = _channels[key];
            if (channel is IInputChannel<TSignal> match)
                match.Receive(signal);
        }

        public bool CanConsume(string key, Type signalType)
        {
            if (!_channels.ContainsKey(key))
                return false;

            var channel = _channels[key];
            return channel.SignalType == signalType;
        }

        public void SetupInput<T>(string key, Action<T> handler)
        {
            if (_channels.ContainsKey(key))
            {
                Debug.LogError($"An input with key {key} has already been set up. Ignoring.");
                return;
            }

            var channel = new InputChannel<T>();
            channel.AddHandler(handler);
            _channels.Add(key, channel);
        }
    }
}