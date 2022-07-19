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
    public abstract class ListView<T> : View<IList<T>>
    {
        [SerializeField] private View<T> _viewPrefab = default;
        [SerializeField] private Transform _viewParent = default;
        [SerializeField] private List<View<T>> _children = default;

        protected override void Setup(ISetup<IList<T>> setup) 
        {
        }

        protected override void OnDisplay(IList<T> target)
        {
            if (target.Count > _children.Count)
            {
                var difference = target.Count - _children.Count;
                for (int i = 0; i < difference; i++)
                {
                    var newView = Instantiate(_viewPrefab, _viewParent);
                    _children.Add(newView);
                }
            }
            
            for (var i = 0; i < _children.Count; i++)
            {
                var view = _children[i];

                if(i < target.Count)
                    view.Display(target[i]);
                else view.Clear();
            }
        }

        private void Reset()
        {
            _viewParent = transform;
            _children = GetComponentsInChildren<View<T>>().ToList();
        }
    }
}