namespace UniViewV3.Messaging
{
    public readonly struct Connection
    {
        private readonly ITransmitter _transmitter;
        private readonly IReceiver _receiver;

        public Connection(ITransmitter transmitter, IReceiver receiver)
        {
            _transmitter = transmitter;
            _receiver = receiver;
        }

        public void Activate()
        {
            _receiver.StartReceivingFrom(_transmitter);
        }

        public bool IsValid()
        {
            return _receiver.CanReceiveFrom(_transmitter);
        }
    }
}