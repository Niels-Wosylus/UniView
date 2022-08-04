using System;
using UniViewV3.Messaging;
using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content.Input
{


    public interface IContentInputChannelGroupBuilder<TViewModel>
    {
        IChannelGroup Build();
    }
    
    public interface IContentInputChannelGroupSetup<T1, T2, T3, TViewModel>
    {
        
    }

    public interface IContentInputChannelGroupBuilder<T1, T2, T3, TViewModel> 
        : IContentInputChannelGroupSetup<T1?, T2?, T3?, TViewModel?>, IContentInputChannelGroupBuilder<TViewModel?>
    {
        
    }
    
    public class ContentInputInputChannelGroupBuilder<T1, T2, T3, TViewModel> : IContentInputChannelGroupBuilder<T1?, T2?, T3?, TViewModel?>
    {
        private readonly string _groupName;
        private readonly IInputChannelBuilder _channelBuilder1;
        private readonly IInputChannelBuilder _channelBuilder2;
        private readonly IInputChannelBuilder _channelBuilder3;

        public ContentInputInputChannelGroupBuilder(Func<T1?, T2?, T3?, TViewModel?> constructor, 
            IProcessor<TViewModel?> modelProcessor, string groupName, string channelName1, string channelName2, string channelName3)
        {
            _groupName = groupName;
            var modelConstructor = new ViewModelConstructor<T1?, T2?, T3?, TViewModel?>(constructor, modelProcessor);

            _channelBuilder1 = GetChannelBuilder(channelName1, modelConstructor.GetProcessorA());
            _channelBuilder2 = GetChannelBuilder(channelName2, modelConstructor.GetProcessorB());
            _channelBuilder3 = GetChannelBuilder(channelName3, modelConstructor.GetProcessorC());
        }

        private IInputChannelBuilder GetChannelBuilder<T>(string channelName, IProcessor<T?> processor)
        {
            var channelKey1 = new ChannelKey(_groupName, channelName);
            return new InputChannelBuilder<T?>(channelKey1, processor);
        }

        public IChannelGroup Build()
        {
            var channel1 = _channelBuilder1.Build();
            var channel2 = _channelBuilder2.Build();
            var channel3 = _channelBuilder3.Build();

            return new ContentInputChannelGroup(_groupName, channel1, channel2, channel3);
        }
    }
}