using System;
using System.Collections.Generic;
using UniViewV3.Content.Input;
using UniViewV3.Content.Output;
using UniViewV3.Messaging.Broadcasting;
using UniViewV3.Messaging.Channels;

namespace UniViewV3
{
    public interface IViewBuilder<TViewModel> : IViewSetup<TViewModel>
    {
        IView<TViewModel> Build();
    }
    
    public class ViewBuilder<TViewModel> : IViewBuilder<TViewModel>
    {
        private const string DefaultContentOutputGroupName = "Default";
        private const string DefaultContentInputGroupName = "Default";
        
        private readonly List<IContentOutputChannelBuilder<TViewModel>> _modelBoundContentOutputBuilders =
            new List<IContentOutputChannelBuilder<TViewModel>>();

        private readonly List<IContentInputChannelGroupBuilder<TViewModel>> _contentInputGroupBuilders =
            new List<IContentInputChannelGroupBuilder<TViewModel>>();

        private readonly IBroadcasterFactory _broadcasterFactory = new BroadcasterFactory();
        
        public IContentOutputChannelSetup<TExtracted> Content<TExtracted>(string channelName, Func<TViewModel, TExtracted> extractor)
        {
            var channelKey = new ChannelKey(DefaultContentOutputGroupName, channelName);
            var builder = new ContentOutputChannelBuilder<TViewModel, TExtracted>(channelKey, extractor, _broadcasterFactory);
            _modelBoundContentOutputBuilders.Add(builder);
            return builder;
        }

        public IContentInputChannelGroupSetup<TIn1, TIn2, TIn3, TViewModel> ModelConstructor<TIn1, TIn2, TIn3>(string channelName1,
            string channelName2, string channelName3, Func<TIn1, TIn2, TIn3, TViewModel> constructor)
        {
            //var builder = new ContentInputInputChannelGroupBuilder<TIn3, TIn2, TIn3, TViewModel>(constructor, )
            throw new NotImplementedException();
        }

        public IView<TViewModel> Build()
        {
            throw new NotImplementedException();
        }
    }
}