namespace UniViewV3.Messaging.Chaining
{
    public abstract class ChainableConverter<TIn, TOut> : ChainableProcessor<TIn?, TOut?>
    {
        protected override void Process(TIn? message, IProcessor<TOut?> continuation)
        {
            if (message == null)
            {
                continuation.Process(default);
                return;
            }
            
            var converted = Convert(message);
            continuation.Process(converted);
        }

        protected abstract TOut Convert(TIn input);
    }
}