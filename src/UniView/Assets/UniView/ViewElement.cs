using UnityEngine;
using UniView.Exposure;
using UniView.Tools;

namespace UniView
{
    public abstract class ViewElementBase : MonoBehaviour, IContentConsumer
    {
        [ViewKey]
        [SerializeField] private string _key;
        
        public abstract void Consume<TContent>(TContent content);
        public abstract void Clear();
        
        public void RegisterIn(IContentConsumerRegistry registry)
        {
            registry.Register(this, _key);
        }
    }
    
    public abstract class ViewElement<T> : ViewElementBase, IDisplay<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            if(content is T match)
                Display(match);
        }

        public abstract void Display(T content);
    }

    public abstract class ViewElement<T, T1> : ViewElement<T>, IDisplay<T1>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                case T1 match1:
                    Display(match1);
                    break;
            }
        }
        
        public abstract void Display(T1 content);
    }
}