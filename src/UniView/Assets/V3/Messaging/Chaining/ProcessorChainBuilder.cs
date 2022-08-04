using System.Collections.Generic;
using System.Linq;
using UniViewV3.Messaging.Broadcasting;

namespace UniViewV3.Messaging.Chaining
{
    public interface IProcessorChainBuilder
    {
        void Add(IChainableProcessorSupplier processor);
        bool ValidateForInputOf<T>();
        bool ValidateForOutputOf<T>();
        IReceiver BuildWithKnownOutput<T>(IProcessor<T> callback);
        ITransmittingProcessor<T> BuildWithKnownInput<T>(ITransmittingProcessorFactory transmitterFactory);
    }
    
    public class ProcessorChainBuilder : IProcessorChainBuilder
    {
        private readonly List<IChainableProcessorSupplier> _suppliers = new List<IChainableProcessorSupplier>();

        public void Add(IChainableProcessorSupplier processor)
        {
            _suppliers.Add(processor);
        }

        public bool ValidateForInputOf<T>()
        {
            throw new System.NotImplementedException();
        }

        public bool ValidateForOutputOf<T>()
        {
            throw new System.NotImplementedException();
        }

        public IReceiver BuildWithKnownOutput<T>(IProcessor<T> callback)
        {
            var head = new ChainableReceiver<T>();

            IChainableProcessor current = head;
            foreach (var supplier in _suppliers)
            {
                current = current.Link(supplier);
            }
            
            var tail = new ProcessorSupplier<T>(callback);
            current.Link(tail);
            return head;
        }

        public ITransmittingProcessor<T> BuildWithKnownInput<T>(ITransmittingProcessorFactory transmitterFactory)
        {
            var head = _suppliers.First().SupplyProcessorOf<T>();
            
            IChainableProcessor current = head;
            foreach (var supplier in _suppliers)
            {
                current = current.Link(supplier);
            }
            
            var tail = current.Link(transmitterFactory);
            return new TransmittingProcessor<T>(head, (ITransmitter)tail);
        }
    }

    public static class ProcessChainerBuilderExtensions
    {
        public static void Add<T>(this IProcessorChainBuilder builder, IChainableProcessor<T> processor)
        {
            var supplier = new ChainableProcessorSupplier<T>(processor);
            builder.Add(supplier);
        }
    }
}