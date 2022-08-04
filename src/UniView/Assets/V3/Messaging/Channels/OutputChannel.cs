using UniViewV3.Messaging.Broadcasting;

namespace UniViewV3.Messaging.Channels
{
    public interface IOutputChannel : IChannel, ITransmitter
    {
        
    }

    public interface IOutputChannel<in TIn> : IOutputChannel, IProcessor<TIn?>
    {
        
    }

    public class OutputChannel<TIn> : IOutputChannel<TIn?>
    {
        public ChannelKey Key { get; }
        private readonly ITransmittingProcessor<TIn?> _broadcaster;
        private TIn? _latestInput;

        public OutputChannel(ChannelKey key, ITransmittingProcessor<TIn?> broadcaster)
        {
            Key = key;
            _broadcaster = broadcaster;
        }

        public void Process(TIn? message)
        {
            _latestInput = message;
            _broadcaster.Process(message);
        }
        
        public void Refresh()
        {
            Process(_latestInput);
        }

        public bool CanTransmitTo<TAny>(IProcessor<TAny?> receiver)
        {
            return _broadcaster.CanTransmitTo(receiver);
        }

        public void StartTransmittingTo<TAny>(IProcessor<TAny?> receiver)
        {
            _broadcaster.StartTransmittingTo(receiver);
        }

        public void RetransmitTo<T>(IProcessor<T?> receiver)
        {
            _broadcaster.RetransmitTo(receiver);
        }
    }
}