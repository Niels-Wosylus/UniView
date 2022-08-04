using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content
{
    public struct ContentDestination
    {
        public IContentConsumer Consumer;
        public ChannelKey ChannelKey;
    }
}