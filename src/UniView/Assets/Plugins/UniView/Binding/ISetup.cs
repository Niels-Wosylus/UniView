using System;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Binding.Signals;

namespace Wosylus.UniView.Binding
{
    public interface ISetup<T>
    {
        IContentChannelSetup<T> Content<TContent>(string key, Func<T, TContent> function);
        void Input<TSignal>(string key, Action<TSignal> handler);
        ISignalSender<TSignal> Output<TSignal>(string key);
        void Output<TSignal>(string key, out ISignalSender<TSignal> output);
    }
}