namespace UniViewV3.Messaging.Chaining
{
    public interface IChainableProcessorSupplier
    {
        bool CanSupplyProcessorOf<T>();
        IChainableProcessor<T> SupplyProcessorOf<T>();
    }

    public class ChainableProcessorSupplier<T> : IChainableProcessorSupplier
    {
        private readonly IChainableProcessor<T> _processor;

        public ChainableProcessorSupplier(IChainableProcessor<T> processor)
        {
            _processor = processor;
        }

        public bool CanSupplyProcessorOf<TAny>()
        {
            return typeof(T) == typeof(TAny);
        }

        public IChainableProcessor<TAny> SupplyProcessorOf<TAny>()
        {
            return (IChainableProcessor<TAny>)_processor;
        }
    }
}