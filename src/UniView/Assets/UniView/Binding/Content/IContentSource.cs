using System.Collections.Generic;

namespace UniView.Binding.Content
{
    public interface IContentSource
    {
        public bool KeyIsAvailable(string key, IContentConsumer consumer);
        IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);
    }
}