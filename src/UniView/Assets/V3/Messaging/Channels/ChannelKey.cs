using System;

namespace UniViewV3.Messaging.Channels
{
    [Serializable]
    public struct ChannelKey
    {
        public string GroupName;
        public string ChannelName;
        public int Index;

        public ChannelKey(string groupName, string channelName)
        {
            GroupName = groupName;
            ChannelName = channelName;
            Index = -1;
        }
        
        public ChannelKey(string groupName, string channelName, int index)
        {
            GroupName = groupName;
            ChannelName = channelName;
            Index = index;
        }

        public static ChannelKey Empty => new ChannelKey("", "", -1);
    }
}