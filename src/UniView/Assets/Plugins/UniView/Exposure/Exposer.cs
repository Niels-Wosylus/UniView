using System;
using System.Collections.Generic;

namespace Wosylus.UniView.Exposure
{
    internal interface IExposer<T>
    {
        Type ExposedType { get; }
        IEnumerable<string> GetKeys();
    }

    internal class Exposer<T, TOutput> : IExposer<T>
    {
        private readonly Dictionary<string, Func<T, TOutput>> _functions = new Dictionary<string, Func<T, TOutput>>();
            
        public Type ExposedType => typeof(TOutput);

        public void AddExposure(string key, Func<T, TOutput> function)
        {
            _functions.Add(key, function);
        }

        public TOutput GetValue(string key, T target)
        {
            return _functions[key].Invoke(target);
        }

        public IEnumerable<string> GetKeys()
        {
            return _functions.Keys;
        }

        public bool Exposes(string key)
        {
            return _functions.ContainsKey(key);
        }
    }
}
