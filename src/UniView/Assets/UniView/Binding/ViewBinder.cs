using System;
using UniView.Binding.Content;
using UniView.Binding.Signals;

namespace UniView.Binding
{
    public interface IViewBinder<T> : ISetup<T>
    {
        
    }
    
    public class ViewBinder<T> : IViewBinder<T>
    {
        private readonly IContentBroadcaster<T> _contentBroadcaster;
        private readonly ISignalBroadcaster _signalBroadcaster;
        private readonly ISignalReceiver _signalReceiver;
        
        public IContentChannelSetup<T> Content<TExposed>(string key, Func<T, TExposed> function)
        {
            return _contentBroadcaster.Content(key, function);
        }
    }
}