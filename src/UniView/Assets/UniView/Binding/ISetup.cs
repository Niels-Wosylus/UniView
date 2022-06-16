using System;

namespace UniView.Binding
{
    public interface ISetup<T>
    {
        IContentChannelSetup<T> Content<TExposed>(string key, Func<T, TExposed> function);
    }
}