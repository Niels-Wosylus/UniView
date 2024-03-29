﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wosylus.UniView.Binding.Content
{
    public interface IContentConsumerRegistry
    {
        void Register(IContentConsumer consumer, string key);
    }

    public interface IContentBroadcaster<T> : IDisplay<T>, IContentConsumerRegistry, IContentSource, IDisposable
    {
        IContentChannelSetup<T, TExposed> SetupContent<TExposed>(string key, Func<T, TExposed> function);
        void OverrideContentChannelController(string key, IContentChannelController<T> controller);
    }
    
    public class ContentBroadcaster<T> : IContentBroadcaster<T>
    {
        private readonly Dictionary<string, IContentChannel<T>> _channels 
            = new Dictionary<string, IContentChannel<T>>();
        
        private readonly Dictionary<string, IContentChannelController<T>> _controllerOverrides
            = new Dictionary<string, IContentChannelController<T>>();

        private readonly HashSet<IContentChannelController<T>> _activeControllers
            = new HashSet<IContentChannelController<T>>();

        private readonly MonoBehaviour _context;
        
        public ContentBroadcaster(MonoBehaviour context)
        {
            _context = context;
        }

        public IContentChannelSetup<T, TExposed> SetupContent<TExposed>(string key, Func<T, TExposed> function)
        {
            if (_channels.ContainsKey(key))
                throw new Exception($"Key {key} is already being used to expose content");

            var channel = new ContentChannel<T, TExposed>(key, function);
            _channels.Add(key, channel);
            return new ContentChannelSetup<T, TExposed>(channel, this);
        }

        public void OverrideContentChannelController(string key, IContentChannelController<T> controller)
        {
            _controllerOverrides[key] = controller;
        }

        public void Register(IContentConsumer consumer, string key)
        {
            if (!_channels.ContainsKey(key))
            {
                Debug.LogWarning($"Cannot register consumer, key {key} is not exposed");
                return;
            }

            _controllerOverrides.TryAdd(key, new ContentChannelController<T>());

            var controller = _controllerOverrides[key];
            _activeControllers.Add(controller);

            var channel = _channels[key];
            channel.RegisterConsumer(consumer);
            controller.Init(channel, _context);
        }

        public void Display(T content)
        {
            foreach (var controller in _activeControllers)
            {
                controller.OnInputChanged(content);
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