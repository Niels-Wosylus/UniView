using System;
using System.Collections.Generic;

namespace Wosylus.UniView.Exposure
{
    internal interface IPropertyExposer
    {
        /// <summary>
        /// Returns a value that this View exposes from the object it displays
        /// </summary>
        T GetExposedValue<T>(string key);
        
        /// <summary>
        /// Returns the Type of the value exposed through the given key
        /// </summary>
        Type GetExposedType(string key);

        /// <summary>
        /// Whether the view exposes a particular type
        /// </summary>
        bool DirectlyExposesType(Type type);

        /// <summary>
        /// Whether the given key exposes a property
        /// </summary>
        bool ExposesKey(string key);

        /// <summary>
        /// Returns all keys for exposed properties of the given type
        /// </summary>
        IEnumerable<string> GetExposedKeys(Type type);

        void UpdateChildList();
    }
}