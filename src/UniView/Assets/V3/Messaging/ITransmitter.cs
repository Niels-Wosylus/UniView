namespace UniViewV3.Messaging
{
    public interface ITransmitter
    {
        bool CanTransmitTo<T>(IProcessor<T?> receiver);
        void StartTransmittingTo<T>(IProcessor<T?> receiver);
        void RetransmitTo<T>(IProcessor<T?> receiver);
    }
}