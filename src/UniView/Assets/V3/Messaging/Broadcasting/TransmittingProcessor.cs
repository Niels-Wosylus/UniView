namespace UniViewV3.Messaging.Broadcasting
{
    public interface ITransmittingProcessor<in T> : ITransmitter, IProcessor<T?>
    {
        
    }

    public class TransmittingProcessor<T> : ITransmittingProcessor<T?>
    {
        private readonly IProcessor<T?> _processor;
        private readonly ITransmitter _transmitter;

        public TransmittingProcessor(IProcessor<T?> processor, ITransmitter transmitter)
        {
            _processor = processor;
            _transmitter = transmitter;
        }

        public bool CanTransmitTo<TAny>(IProcessor<TAny?> receiver)
        {
            return _transmitter.CanTransmitTo(receiver);
        }

        public void StartTransmittingTo<TAny>(IProcessor<TAny?> receiver)
        {
            _transmitter.StartTransmittingTo(receiver);
        }

        public void RetransmitTo<T1>(IProcessor<T1?> receiver)
        {
            _transmitter.RetransmitTo(receiver);
        }

        public void Process(T? message)
        {
            
            _processor.Process(message);
        }
    }
}