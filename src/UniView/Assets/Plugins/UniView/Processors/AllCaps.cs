using Wosylus.UniView.Binding.Content.Processors;

namespace Wosylus.UniView.Processors
{
    public class AllCaps : ViewContentProcessor<string>
    {
        private string _mostRecentInput;
        private string _mostRecentOutput;
        
        protected override string Process(string input)
        {
            if (_mostRecentInput == input)
                return _mostRecentOutput;
            
            _mostRecentInput = input.ToUpper();
            return _mostRecentOutput;
        }
    }
}