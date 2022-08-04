namespace UniViewV3.Messaging.Chaining
{
    public interface IProcessorSupplier
    {
        bool CanSupplyProcessorOf<T>();
        IProcessor<T> SupplyProcessorOf<T>();
    }
    
    public class ProcessorSupplier<T> : IProcessorSupplier
    {
        private readonly IProcessor<T> _processor;

        public ProcessorSupplier(IProcessor<T> processor)
        {
            _processor = processor;
        }

        public bool CanSupplyProcessorOf<TAny>()
        {
            return typeof(T) == typeof(TAny);
        }

        public IProcessor<TAny> SupplyProcessorOf<TAny>()
        {
            return (IProcessor<TAny>)_processor;
        }
    }
}