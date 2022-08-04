using System;
using System.Collections.Generic;
using UniViewV3.Messaging.Broadcasting;
using UniViewV3.Messaging.Chaining;
using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content.Output
{
    public interface IContentOutputChannelSetup
    {
        void AddProcessor(IChainableProcessorSupplier processor);
    }
    
    public interface IContentOutputChannelSetup<out TExtracted> : IContentOutputChannelSetup
    {
        void AddRefresher(IChannelRefresher<TExtracted> refresher);
    }

    public interface IContentOutputChannelBuilder<in TViewModel>
    {
        IOutputChannel<TViewModel> Build();
    }
    
    public interface IContentOutputChannelBuilder<in TViewModel, out TExtracted> 
        : IContentOutputChannelSetup<TExtracted?>, IContentOutputChannelBuilder<TViewModel?>
    {
        
    }

    public class ContentOutputChannelBuilder<TViewModel, TExtracted> : IContentOutputChannelBuilder<TViewModel?, TExtracted?>
    {
        private readonly ChannelKey _key;
        private readonly Func<TViewModel, TExtracted> _extractor;
        private readonly List<IChainableProcessorSupplier> _suppliers = new List<IChainableProcessorSupplier>();
        private readonly List<IChannelRefresher<TExtracted?>> _refreshers = new List<IChannelRefresher<TExtracted?>>();
        private readonly IBroadcasterFactory _broadcasterFactory;

        public ContentOutputChannelBuilder(ChannelKey key, Func<TViewModel, TExtracted> extractor, IBroadcasterFactory broadcasterFactory)
        {
            _key = key;
            _extractor = extractor;
            _broadcasterFactory = broadcasterFactory;
        }

        public void AddProcessor(IChainableProcessorSupplier processor)
        {
            _suppliers.Add(processor);
        }

        public void AddRefresher(IChannelRefresher<TExtracted?> refresher)
        {
            _refreshers.Add(refresher);
        }

        public IOutputChannel<TViewModel?> Build()
        {
            var chainBuilder = new ProcessorChainBuilder();
            
            AddExtractor(chainBuilder);
            AddRefresherNotifier(chainBuilder);
            AddProcessors(chainBuilder);

            var transmitter = chainBuilder.BuildWithKnownInput<TViewModel?>(_broadcasterFactory);
            return new OutputChannel<TViewModel?>(_key, transmitter);
        }

        private void AddExtractor(IProcessorChainBuilder chainBuilder)
        {
            chainBuilder.Add(new ContentExtractor<TViewModel, TExtracted>(_extractor));
        }
        
        private void AddRefresherNotifier(IProcessorChainBuilder chainBuilder)
        {
            if (_refreshers.Count > 0)
                chainBuilder.Add(new RefresherNotifier<TExtracted?>(_refreshers));
        }
        
        private void AddProcessors(IProcessorChainBuilder chainBuilder)
        {
            foreach (var supplier in _suppliers)
            {
                chainBuilder.Add(supplier);
            }
        }
    }
}