using UniViewV3.Messaging;
using UniViewV3.Messaging.Channels;

namespace UniViewV3.Content.Management
{
    public interface IContentChannelManager<TViewModel> : IProcessor<TViewModel?>, IDisplay<TViewModel?>, IRefreshable
    {
        void AddContentReceiver(ChannelKey key);
    }
    
    //Can clear view
    //Can add new receivers
    
    public class ContentChannelManager<TViewModel> : IContentChannelManager<TViewModel?>
    {
        public bool IsDisplayingContent => DisplayedContent != null;
        public TViewModel? DisplayedContent { get; private set; }

        public void Display(TViewModel? content)
        {
            //Can display given input (view model) by passing it to default content input channel
            DisplayedContent = content;
            throw new System.NotImplementedException();
        }

        public void Clear()
        {
            //how do we do this? what happens when a view with multiple content sources have just one of them cleared?
            throw new System.NotImplementedException();
        }

        public void Refresh()
        {
            //Can refresh view by calling for input channels to refresh. This will trigger new construction of view model
            throw new System.NotImplementedException();
        }
        
        public void Process(TViewModel? message)
        {
            //Trigger OnDisplay callbacks
            //Push to content output channels
            
            throw new System.NotImplementedException();
        }
        
        public void AddContentReceiver(ChannelKey key)
        {
            throw new System.NotImplementedException();
        }
    }
}