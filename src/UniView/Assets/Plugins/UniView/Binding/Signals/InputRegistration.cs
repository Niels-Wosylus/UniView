using System;

namespace Wosylus.UniView.Binding.Signals
{
    [Serializable]
    public struct InputRegistration
    {
        public string InputKey;
        public string SourceKey;
        public ViewElementBase Source;
    }
}