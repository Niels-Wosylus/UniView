using System.Collections.Generic;
using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content.Input
{
    public class ContentInputChannelGroup : IChannelGroup
    {
        public string Name { get; }
        private readonly IList<IInputChannel> _channels;

        public ContentInputChannelGroup(string name, params IInputChannel[] channels)
        {
            Name = name;
            _channels = channels;
        }
    }
}