using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content
{
    public struct ContentSource
    {
        public IContentProducer Producer;
        public ChannelKey ChannelKey;
    }
}