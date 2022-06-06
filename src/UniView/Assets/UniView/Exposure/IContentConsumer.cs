using System;

namespace UniView.Exposure
{
    public interface IContentConsumer
    {
        void RegisterIn(IContentConsumerRegistry registry);
        void Consume<TContent>(TContent content);
        bool CanConsume(Type contentType);
        void Clear();
    }
}