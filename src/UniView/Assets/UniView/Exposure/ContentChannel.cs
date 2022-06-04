using System;
using System.Collections.Generic;

namespace UniView.Exposure
{
    public interface IContentExposure
    {
        void Continuously();
    }
    
    public interface IContentChannel : IContentExposure, IDisposable
    {
        void RegisterConsumer(IContentConsumer consumer);
        void RegisterDisposable(IDisposable subscription);
        void RefreshConsumers();
        void ClearConsumers();
    }
    
    public class ContentChannel<TIn, TOut> : IContentChannel
    {
        private readonly List<IContentConsumer> _consumers = new List<IContentConsumer>(8);
        private readonly List<IDisposable> _disposables = new List<IDisposable>();
        private readonly Func<TIn, TOut> _function;
        private readonly IContentProvider<TIn> _contentProvider;

        public ContentChannel(IContentProvider<TIn> contentProvider, Func<TIn, TOut> function)
        {
            _contentProvider = contentProvider;
            _function = function;
        }

        public void RegisterConsumer(IContentConsumer consumer)
        {
            _consumers.Add(consumer);
        }

        public void RegisterDisposable(IDisposable subscription)
        {
            _disposables.Add(subscription);
        }

        public void RefreshConsumers()
        {
            var input = _contentProvider.GetContent();
            var content = _function.Invoke(input);
            foreach (var consumer in _consumers)
            {
                consumer.Consume(content);
            }
        }

        public void ClearConsumers()
        {
            foreach (var consumer in _consumers)
            {
                consumer.Clear();
            }
        }

        public void Dispose()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
        
        public void Continuously()
        {
            var subscription = GlobalViewUpdater.Register(RefreshConsumers);
            RegisterDisposable(subscription);
        }
    }
}