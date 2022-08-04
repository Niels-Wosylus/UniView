namespace UniViewV3.Messaging.Chaining
{
    public interface IChainableReceiver : IChainableProcessor, IReceiver
    {
        
    }
    
    public class ChainableReceiver<TIn> : ChainableProcessor<TIn>, IChainableReceiver
    {
        private ITransmitter? _transmitter;
        
        protected override void Process(TIn? message, IProcessor<TIn?> continuation)
        {
            continuation.Process(message);
        }

        public bool CanReceiveFrom(ITransmitter transmitter)
        {
            return transmitter.CanTransmitTo(this);
        }

        public void StartReceivingFrom(ITransmitter transmitter)
        {
            transmitter.StartTransmittingTo(this);
            _transmitter = transmitter;
        }

        public void RequestRetransmissionFromTransmitter()
        {
            _transmitter?.RetransmitTo(this);
        }
    }
}