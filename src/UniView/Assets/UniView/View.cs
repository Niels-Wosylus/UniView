using System;
using UniView.Exposure;

namespace UniView
{
    public abstract class View<T> : ViewElement<T>, IContentProducer
    {
        protected sealed override void Display(T content)
        {
            throw new System.NotImplementedException();
        }
        
        public sealed override void Clear()
        {
            throw new System.NotImplementedException();
        }
        
        protected abstract void ExposeContents();
        
        protected IContentExposure Expose<TExposed>(string key, Func<T, TExposed> exposer)
        {
            throw new System.NotImplementedException();
        }
    }
}