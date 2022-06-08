namespace UniView.Binding
{
    public interface IContentChannelSetup<T>
    {
        void Continuously();
        void OverrideController(IContentChannelController<T> controller);
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
            OverrideController(new RefreshContinuously<T>());
        }

        public void OverrideController(IContentChannelController<T> controller)
        {
            var key = _channel.Key;
            _overrider.OverrideContentChannelController(key, controller);
        }
    }
}