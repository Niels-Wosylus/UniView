using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniView.Binding;
using UniView.Tools;
using UniView.Utilities;

namespace UniView
{
    [DisallowMultipleComponent]
    public abstract class ViewBase : ViewElementBase, IContentProducer
    {
        [Header("Children")]
        [ReadOnly]
        [SerializeField] private ViewElementBase[] _elements = Array.Empty<ViewElementBase>();
        
        public abstract bool KeyIsAvailable(string key, IContentConsumer consumer);
        public abstract IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);

#if UNITY_EDITOR
        public override void OnValidate()
        {
            base.OnValidate();
            SetElements();
        }

        private void SetElements()
        {
            var elements = GetComponentsInChildren<ViewElementBase>()
                .Where(child => !ReferenceEquals(child, this))
                .Where(child => ReferenceEquals(child.FindParent(), this));

            _elements = elements.ToArray();
        }
#endif
    }
}