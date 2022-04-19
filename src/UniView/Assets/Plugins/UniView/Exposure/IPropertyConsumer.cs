namespace Wosylus.UniView.Exposure
{
    internal interface IPropertyConsumer
    {
        void ConsumeFrom(IPropertyExposer exposer);
        IPropertyExposer GetParent();
        void Clear();
    }
}