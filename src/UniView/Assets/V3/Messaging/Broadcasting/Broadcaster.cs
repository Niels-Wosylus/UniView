using System.Collections.Generic;

namespace UniViewV3.Messaging.Broadcasting
{
    public interface IBroadcaster<in T> : ITransmittingProcessor<T?>
    {
        
    }

    public class Broadcaster<T> : IBroadcaster<T?>
    {
        private readonly List<IProcessor<T?>> _receivers = new List<IProcessor<T?>>();
        private T? _latestTransmission;

        public bool CanTransmitTo<TAny>(IProcessor<TAny?> receiver)
        {
            return typeof(T).IsAssignableFrom(typeof(TAny?));
        }

        public void StartTransmittingTo<TAny>(IProcessor<TAny?> receiver)
        {
            if (typeof(TAny?) == typeof(T?))
                Connect((IProcessor<T?>)receiver);
            else Connect(new CastingAdapter<T?, TAny?>(receiver));
        }

        public void RetransmitTo<TAny>(IProcessor<TAny?> receiver)
        {
            if(_latestTransmission is TAny message)
                receiver.Process(message);
            else receiver.Process(default);
        }

        public void Process(T? message)
        {
            _latestTransmission = message;
            TransmitToReceivers(message);
        }

        private void TransmitToReceivers(T? message)
        {
            foreach (var receiver in _receivers)
            {
                receiver.Process(message);
            }
        }

        private void Connect(IProcessor<T?> receiver)
        {
            _receivers.Add(receiver);
        }
    }
}