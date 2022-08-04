namespace UniViewV3.Messaging.Broadcasting
{
    public interface IBroadcasterFactory : ITransmittingProcessorFactory
    {
        
    }
    
    public class BroadcasterFactory : IBroadcasterFactory
    {
        public bool CanSupplyProcessorOf<T>()
        {
            return true;
        }

        public IProcessor<T> SupplyProcessorOf<T>()
        {
            return new Broadcaster<T>();
        }
    }
}