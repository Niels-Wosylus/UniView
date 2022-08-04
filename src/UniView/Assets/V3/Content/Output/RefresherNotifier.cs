using System.Collections.Generic;
using UniViewV3.Messaging;
using UniViewV3.Messaging.Chaining;
using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content.Output
{
    public class RefresherNotifier<TExtracted> : ChainableProcessor<TExtracted>
    {
        private readonly IEnumerable<IChannelRefresher<TExtracted?>> _refreshers;

        public RefresherNotifier(IEnumerable<IChannelRefresher<TExtracted?>> refreshers)
        {
            _refreshers = refreshers;
        }

        protected override void Process(TExtracted? message, IProcessor<TExtracted?> continuation)
        {
            NotifyRefreshers(message);
            continuation.Process(message);
        }
        
        private void NotifyRefreshers(TExtracted? message)
        {
            foreach (var refresher in _refreshers)
            {
                refresher.OnContentChanged(message);
            }
        }
    }
}