using System;

namespace UniView.Binding
{
    public interface IContentConsumer
    {
        void RegisterIn(IContentConsumerRegistry registry);
        void Consume<TContent>(TContent content);
        bool CanConsume(Type contentType);
        void Clear();
    }
}