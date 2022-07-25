using System;
using System.Collections.Generic;

namespace Wosylus.UniView.Binding.Content.Processors
{
    public interface IContentProcessorChain
    {
        void Process<T>(T content);
        bool CanProcess(Type inputType);
    }
    
    public class ContentProcessorChain : IContentProcessorChain
    {
        private readonly IList<IContentProcessor> _processors;

        public ContentProcessorChain(IList<IContentProcessor> processors)
        {
            _processors = processors;
        }

        public void Process<T>(T content)
        {
            ContentProcess.Run(content, _processors);
        }

        public bool CanProcess(Type inputType)
        {
            foreach (var processor in _processors)
            {
                if (!processor.CanProcess(inputType))
                    return false;

                inputType = processor.GetOutputType(inputType);
            }

            return true;
        }
    }
}