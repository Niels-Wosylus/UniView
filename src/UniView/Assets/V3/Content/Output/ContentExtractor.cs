using System;
using System.Collections.Generic;
using UniViewV3.Messaging;
using UniViewV3.Messaging.Chaining;

namespace UniViewV3.Content.Output
{
    public class ContentExtractor<TViewModel, TExtracted> : ChainableProcessor<TViewModel?, TExtracted?>
    {
        private readonly Func<TViewModel, TExtracted> _extractor;
        private TExtracted? _latestExtracted;

        public ContentExtractor(Func<TViewModel, TExtracted> extractor)
        {
            _extractor = extractor;
        }

        protected override void Process(TViewModel? message, IProcessor<TExtracted?> continuation)
        {
            if (message == null)
            {
                continuation.Process(default);
                return;
            }
            
            var extracted = _extractor.Invoke(message);
            if (PreviouslyExtractedEquals(extracted))
                return;
            
            _latestExtracted = extracted;
            continuation.Process(extracted);
        }
        
        private bool PreviouslyExtractedEquals(TExtracted? extracted)
        {
            return EqualityComparer<TExtracted?>.Default.Equals(extracted, _latestExtracted);
        }
    }
}