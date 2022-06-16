using System.Collections.Generic;

namespace UniView.Binding
{
    public interface IContentSource
    {
        public bool KeyIsAvailable(string key, IContentConsumer consumer);
        IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);
    }
}