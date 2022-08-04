namespace UniViewV3.Messaging.Channels
{
    public interface IInputChannel : IChannel, IReceiver
    {
        
    }

    public class InputChannel : IInputChannel
    {
        public ChannelKey Key { get; }
        private readonly IReceiver _receiver;

        public InputChannel(ChannelKey key, IReceiver receiver)
        {
            Key = key;
            _receiver = receiver;
        }

        public bool CanReceiveFrom(ITransmitter transmitter)
        {
            return _receiver.CanReceiveFrom(transmitter);
        }

        public void StartReceivingFrom(ITransmitter transmitter)
        {
            _receiver.StartReceivingFrom(transmitter);
        }

        public void RequestRetransmissionFromTransmitter()
        {
            _receiver.RequestRetransmissionFromTransmitter();
        }

        public void Refresh()
        {
            RequestRetransmissionFromTransmitter();
        }
    }
}