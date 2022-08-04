namespace UniViewV3.Messaging.Channels
{
    public interface IChannelRefresher<in TExtracted>
    {
        void Init(IRefreshable channel);
        void OnContentChanged(TExtracted content);
    }
}