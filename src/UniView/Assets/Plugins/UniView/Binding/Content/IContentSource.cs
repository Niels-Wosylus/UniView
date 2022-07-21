using System.Collections.Generic;

namespace Wosylus.UniView.Binding.Content
{
    public interface IContentSource
    {
        IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);
    }
}