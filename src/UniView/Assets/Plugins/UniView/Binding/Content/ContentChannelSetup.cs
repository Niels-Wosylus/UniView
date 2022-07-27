namespace Wosylus.UniView.Binding.Content
{
    public interface IContentChannelSetup<TIn, TOut>
    {
        void Continuously();
        void WithController(IContentChannelController<TIn> controller);
    }

    public class ContentChannelSetup<TInput, TOutput> : IContentChannelSetup<TInput, TOutput>
    {
        private readonly IContentBroadcaster<TInput> _overrider;
        private readonly IContentChannel<TInput> _channel;

        public ContentChannelSetup(IContentChannel<TInput> channel, IContentBroadcaster<TInput> overrider)
        {
            _channel = channel;
            _overrider = overrider;
        }

        public void Continuously()
        {
            WithController(new RefreshContinuously<TInput>());
        }

        public void WithController(IContentChannelController<TInput> controller)
        {
            var key = _channel.Key;
            _overrider.OverrideContentChannelController(key, controller);
        }
    }
}