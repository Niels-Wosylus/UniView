namespace UniViewV3.Messaging
{
    public interface IReceiver
    {
        bool CanReceiveFrom(ITransmitter transmitter);
        void StartReceivingFrom(ITransmitter transmitter);
        void RequestRetransmissionFromTransmitter();
    }
}