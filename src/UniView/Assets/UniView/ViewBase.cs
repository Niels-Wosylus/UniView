using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniView.Binding;
using UniView.Utilities;

namespace UniView
{
    public abstract class ViewBase : ViewElementBase, IContentProducer
    {
        [SerializeField] private ViewElementBase[] _elements = Array.Empty<ViewElementBase>();
        
        public abstract bool KeyIsAvailable(string key, IContentConsumer consumer);
        public abstract IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);

        public override void OnValidate()
        {
            base.OnValidate();
            var elements = GetComponentsInChildren<ViewElementBase>()
                .Where(child => !ReferenceEquals(child, this))
                .Where(child => ReferenceEquals(child.FindParent(), this));

            _elements = elements.ToArray();
        }
    }
}