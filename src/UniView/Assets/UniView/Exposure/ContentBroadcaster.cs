﻿using System;
using System.Collections.Generic;

namespace UniView.Exposure
{
    public interface ISetup<T>
    {
        IContentChannelSetup<T> Content<TExposed>(string key, Func<T, TExposed> function);
    }
    
    public interface IContentConsumerRegistry
    {
        void Register(IContentConsumer consumer, string key);
    }

    public interface IContentBroadcaster<T> : IDisplay<T>, ISetup<T>, IContentConsumerRegistry, IDisposable
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
    }
}