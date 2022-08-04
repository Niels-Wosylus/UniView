namespace UniViewV3.Messaging.Chaining
{
    public interface IChainableProcessor
    {
        bool CanLink(IChainableProcessorSupplier supplier);
        IChainableProcessor Link(IChainableProcessorSupplier supplier);
        
        bool CanLink(IProcessorSupplier supplier);
        IProcessor Link(IProcessorSupplier supplier);
    }
    
    public interface IChainableProcessor<in T> : IChainableProcessor, IProcessor<T?>
    {

    }

    public abstract class ChainableProcessor<TIn, TOut> : IChainableProcessor<TIn?>
    {
        private IProcessor<TOut?> _continuation = null!;

        public void Process(TIn? message)
        {
            Process(message, _continuation);
        }

        protected abstract void Process(TIn? message, IProcessor<TOut?> continuation);

        public bool CanLink(IChainableProcessorSupplier supplier)
        {
            return supplier.CanSupplyProcessorOf<TOut?>();
        }

        public IChainableProcessor Link(IChainableProcessorSupplier supplier)
        {
            IChainableProcessor<TOut?> processor = supplier.SupplyProcessorOf<TOut?>();
            _continuation = processor;
            return processor;
        }

        public bool CanLink(IProcessorSupplier supplier)
        {
            return supplier.CanSupplyProcessorOf<TOut?>();
        }

        public IProcessor Link(IProcessorSupplier supplier)
        {
            _continuation = supplier.SupplyProcessorOf<TOut?>();
            return _continuation;
        }
    }
    
    public abstract class ChainableProcessor<T> : ChainableProcessor<T?, T?>
    {

    }

    public static class ChainableProcessorExtensions
    {
        public static IChainableProcessor<T> Link<T>(this IChainableProcessor host, IChainableProcessor<T?> processor)
        {
            var supplier = new ChainableProcessorSupplier<T?>(processor);
            host.Link(supplier);
            return processor;
        }
        
        public static void Link<T>(this IChainableProcessor host, IProcessor<T?> processor)
        {
            var supplier = new ProcessorSupplier<T?>(processor);
            host.Link(supplier);
        }
    }
}