using System;

namespace Wosylus.UniView.Binding.Content
{
    public interface IContentChannelSetup<T>
    {
        void Continuously();
        void WithController(IContentChannelController<T> controller);
    }

    public class ContentChannelSetup<T> : IContentChannelSetup<T>
    {
        private readonly IContentBroadcaster<T> _overrider;
        private readonly IContentChannel<T> _channel;

        public ContentChannelSetup(IContentChannel<T> channel, IContentBroadcaster<T> overrider)
        {
            _channel = channel;
            _overrider = overrider;
        }

        public void Continuously()
        {
            WithController(new RefreshContinuously<T>());
        }

        public void WithController(IContentChannelController<T> controller)
        {
            var key = _channel.Key;
            _overrider.OverrideContentChannelController(key, controller);
        }
    }
}