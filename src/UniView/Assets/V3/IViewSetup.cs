using System;
using UniViewV3.Content.Input;
using UniViewV3.Content.Output;

namespace UniViewV3
{
    public interface IViewSetup
    {
        /// <summary>
        /// Exposes content unrelated to the view model
        /// </summary>
        /// <param name="channelName">Name of the channel</param>
        /// <param name="generator">Function used to generated the content</param>
        /// <typeparam name="T">Type of the content to expose</typeparam>
        //IContentOutputChannelSetup<T> UnboundContent<T>(string channelName, Func<T> generator);
    }
    
    public interface IViewSetup<TViewModel> : IViewSetup
    {
        /// <summary>
        /// Exposes content extracted from the view model
        /// </summary>
        /// <param name="channelName">Name of the channel</param>
        /// <param name="extractor">Function used to derive the exposed content from the view model</param>
        /// <typeparam name="TExtracted">Type of the exposed content</typeparam>
        /// <returns></returns>
        IContentOutputChannelSetup<TExtracted> Content<TExtracted>(string channelName, Func<TViewModel, TExtracted> extractor);

        // IContentChannelInputGroupSetup<TIn1, TViewModel> Constructor<TIn1>(string channelGroupName, string channelName1,
        //     Func<TIn1, TViewModel> constructor);
        //
        // IContentChannelInputGroupSetup<TIn1, TIn2, TViewModel> Constructor<TIn1, TIn2>(string channelGroupName,
        //     string channelName1, string channelName2, Func<TIn1, TIn2, TViewModel> constructor);

        IContentInputChannelGroupSetup<TIn1, TIn2, TIn3, TViewModel> ModelConstructor<TIn1, TIn2, TIn3>(
            string channelName1, string channelName2, string channelName3,
            Func<TIn1, TIn2, TIn3, TViewModel> constructor);
    }
}