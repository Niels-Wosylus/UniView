using System.Collections.Generic;
using UniViewV3.Messaging.Chaining;

namespace UniViewV3.Messaging.Channels
{
    public interface IInputChannelSetup
    {
        void AddProcessor(IChainableProcessorSupplier processor);
    }
    
    public interface IInputChannelBuilder : IInputChannelSetup
    {
        IInputChannel Build();
    }
    
    public class InputChannelBuilder<TOut> : IInputChannelBuilder
    {
        private readonly ChannelKey _key;
        private readonly List<IChainableProcessorSupplier> _processors = new List<IChainableProcessorSupplier>();
        private readonly IProcessor<TOut> _outputProcessor;

        public InputChannelBuilder(ChannelKey key, IProcessor<TOut> outputProcessor)
        {
            _outputProcessor = outputProcessor;
            _key = key;
        }

        public void AddProcessor(IChainableProcessorSupplier processor)
        {
            _processors.Add(processor);
        }
        
        public IInputChannel Build()
        {
            var chainBuilder = new ProcessorChainBuilder();
            AddProcessors(chainBuilder);
            var receiver = chainBuilder.BuildWithKnownOutput(_outputProcessor);
            return new InputChannel(_key, receiver);
        }

        private void AddProcessors(IProcessorChainBuilder chainBuilder)
        {
            foreach (var processor in _processors)
            {
                chainBuilder.Add(processor);
            }
        }
    }
}