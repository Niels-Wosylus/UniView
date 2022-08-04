namespace UniViewV3.Messaging.Channels
{
    public interface IChannel : IRefreshable
    {
        ChannelKey Key { get; }
    }
}