using System;
using UniView.Exposure;

namespace UniView
{
    public abstract class ViewBase : ViewElementBase, IContentProducer
    {

    }
    
    public abstract class View<T> : ViewBase, IDisplay<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            if(content is T match)
                Display(match);
        }

        public void Display(T content)
        {
            throw new NotImplementedException();
        }
        
        public sealed override void Clear()
        {
            throw new NotImplementedException();
        }
        
        protected abstract void Setup(ISetup<T> setup);
    }
}