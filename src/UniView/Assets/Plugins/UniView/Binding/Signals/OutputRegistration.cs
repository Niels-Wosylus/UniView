using System;

namespace Wosylus.UniView.Binding.Signals
{
    [Serializable]
    public struct OutputRegistration
    {
        public string OutputKey;
        public string ConsumerKey;
        public ViewBase Consumer;
    }
}