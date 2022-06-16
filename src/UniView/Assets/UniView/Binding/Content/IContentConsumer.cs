using System;

namespace UniView.Binding.Content
{
    public interface IContentConsumer
    {
        void Consume<TContent>(TContent content);
        bool CanConsume(Type contentType);
        void Clear();
    }
}