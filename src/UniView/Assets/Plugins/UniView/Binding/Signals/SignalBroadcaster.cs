using System.Collections.Generic;
using UnityEngine;

namespace Wosylus.UniView.Binding.Signals
{
    public interface ISignalBroadcaster
    {
        ISignalSender<T> SetupOutput<T>(string key);
        void Broadcast<T>(string key, T signal);
    }
    
    public class SignalBroadcaster : ISignalBroadcaster
    {
        private readonly Dictionary<string, IOutputChannel> _channels = new Dictionary<string, IOutputChannel>();

        public ISignalSender<T> SetupOutput<T>(string key)
        {
            if (_channels.ContainsKey(key))
            {
                Debug.LogError($"An output with key {key} has already been set up. Ignoring.");
                return new NullSignalSender<T>();
            }

            var channel = new OutputChannel<T>(key);
            _channels.Add(key, channel);
            return channel;
        }

        public void Broadcast<T>(string key, T signal)
        {
            if (_channels.ContainsKey(key))
            {
                Debug.LogError($"Error broadcasting signal ({typeof(T)}) through key {key}, but output with such key has not been set up");
                return;
            }

            var channel = _channels[key];
            if (channel is not IOutputChannel<T> match)
            {
                Debug.LogError($"Error broadcasting signal ({typeof(T)}) through key {key}, but output with that key has been set up to broadcast {channel.OutputType}");
                return;
            }
            
            match.Send(signal);
        }
    }
}