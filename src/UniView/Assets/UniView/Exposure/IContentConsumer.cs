namespace UniView.Exposure
{
    public interface IContentConsumer
    {
        void RegisterIn(IContentConsumerRegistry registry);
        void Consume<TContent>(TContent content);
        void Clear();
    }
}