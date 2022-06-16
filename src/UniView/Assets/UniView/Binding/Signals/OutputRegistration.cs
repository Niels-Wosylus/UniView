using System;

namespace UniView.Binding.Signals
{
    [Serializable]
    public struct OutputRegistration
    {
        public string OutputKey;
        public string ConsumerKey;
        public ViewBase Consumer;
    }
}