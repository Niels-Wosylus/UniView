using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Wosylus.UniView.Binding;

namespace Wosylus.UniView.Views
{
    /// <summary>
    /// Displays a collection of items
    /// </summary>
    /// <typeparam name="T">The type of item to display</typeparam>
    public abstract class CollectionView<T> : View<IEnumerable<T>>
    {
        [SerializeField] private View<T> _viewPrefab = default;
        [SerializeField] private Transform _viewParent = default;
        [SerializeField] private List<View<T>> _children = default;
        [SerializeField] private int _prewarmAmount = default;

        private readonly List<T> _entries = new List<T>();

        protected override void Setup(ISetup<IEnumerable<T>> setup) 
        {
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            EnsureCapacity(_prewarmAmount);
        }

        protected override void OnDisplay(IEnumerable<T> target)
        {
            CopyEntries(target);
            EnsureCapacity(_entries.Count);
            RefreshViews();
        }

        private void CopyEntries(IEnumerable<T> target)
        {
            _entries.Clear();
            foreach (var entry in target)
            {
                _entries.Add(entry);
            }
        }
        
        private void EnsureCapacity(int capacity)
        {
            var deficit = capacity - _children.Count;
            for (int i = 0; i < deficit; i++)
            {
                AddView();
            }
        }

        private void AddView()
        {
            var newView = Instantiate(_viewPrefab, _viewParent);
            _children.Add(newView);
        }

        private void RefreshViews()
        {
            for (int i = 0; i < _entries.Count; i++)
            {
                _children[i].Display(_entries[i]);
            }
            
            for (int i = _entries.Count; i < _children.Count; i++)
            {
                _children[i].Clear();
            }
        }
        
        private void Reset()
        {
            _viewParent = transform;
            _children = GetComponentsInChildren<View<T>>().ToList();
        }

        public override void OnValidate()
        {
            base.OnValidate();

            if (_viewPrefab == null)
                return;
            
            EnsureCapacity(_prewarmAmount);
        }
    }
}