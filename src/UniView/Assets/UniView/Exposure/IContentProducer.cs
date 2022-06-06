using System.Collections.Generic;

namespace UniView.Exposure
{
    public interface IContentProducer
    {
        public bool KeyIsAvailable(string key, IContentConsumer consumer);
        IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);
    }
}