#nullable enable

namespace UniViewV3.Messaging
{
    /// <summary>
    /// Provides compatability between a transmitter of type A and a receiver of type B.
    /// The adapter assumes that type B is assignable to type A and performs a cast. 
    /// </summary>
    /// <typeparam name="TA">Transmitted type</typeparam>
    /// <typeparam name="TB">Received type</typeparam>
    public class CastingAdapter<TA, TB> : IProcessor<TA?>
    {
        private readonly IProcessor<TB?> _receiver;

        public CastingAdapter(IProcessor<TB?> receiver)
        {
            _receiver = receiver;
        }

        public void Process(TA? message)
        {
            if(message is TB match)
                _receiver.Process(match);
            else
            {
                _receiver.Process(default);
                Log.Warning("Invalid connection detected");
            }
        }
    }
}