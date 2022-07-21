﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wosylus.UniView.Binding.Content;
using Wosylus.UniView.Tools;

namespace Wosylus.UniView
{
    [DisallowMultipleComponent]
    public abstract class ViewBase : ViewElementBase, IContentSource
    {
        [ReadOnly]
        [SerializeField] private ViewElementBase[] _elements = Array.Empty<ViewElementBase>();

        public abstract bool KeyIsAvailable(string key, IContentConsumer consumer);
        public abstract IEnumerable<string> GetAvailableKeysFor(IContentConsumer consumer);

        protected void RegisterElementsIn(IContentConsumerRegistry registry)
        {
            foreach (var element in _elements)
            {
                element.RegisterIn(registry);
            }
        }
        
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
                .Where(child => ReferenceEquals(child.Parent, this));

            _elements = elements.ToArray();
        }
#endif
    }
}