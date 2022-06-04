using UnityEngine;
using UniView.Exposure;
using UniView.Tools;

namespace UniView
{
    public abstract class ViewElement<T> : MonoBehaviour, IContentConsumer
    {
        [ViewKey]
        [SerializeField] private string _key;
        
        public void RegisterIn(IConsumerRegistry registry)
        {
            registry.Register(this, _key);
        }

        public virtual void Consume<TContent>(TContent content)
        {
            if(content is T match)
                Display(match);
        }

        protected abstract void Display(T content);
        
        public abstract void Clear();
    }

    public abstract class ViewElement<T, T1> : ViewElement<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                case T1 convertable:
                    var converted = Convert(convertable);
                    Display(converted);
                    break;
            }
        }

        protected abstract T Convert(T1 content);
    }
    
    public abstract class ViewElement<T, T1, T2> : ViewElement<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                case T1 convertable1:
                    var converted1 = Convert(convertable1);
                    Display(converted1);
                    break;
                case T2 convertable2:
                    var converted2 = Convert(convertable2);
                    Display(converted2);
                    break;
            }
        }

        protected abstract T Convert(T1 content);
        protected abstract T Convert(T2 content);
    }
}