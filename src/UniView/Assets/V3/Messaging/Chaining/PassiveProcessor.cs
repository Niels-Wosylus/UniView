namespace UniViewV3.Messaging.Chaining
{
    public class PassiveProcessor<T> : ChainableProcessor<T>
    {
        protected override void Process(T? message, IProcessor<T?> continuation)
        {
            continuation.Process(message);
        }
    }
}