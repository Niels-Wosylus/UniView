namespace UniView.Exposure
{
    public interface IContentConsumer
    {
        void RegisterIn(IConsumerRegistry registry);
        void Consume<TContent>(TContent content);
        void Clear();
    }
}