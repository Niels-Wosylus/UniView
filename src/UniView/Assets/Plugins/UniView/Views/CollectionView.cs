using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Wosylus.UniView.Views
{
    public readonly struct CollectionViewEventArgs<T> where T : class
    {
        public CollectionViewEventArgs(View<T> view)
        {
            View = view;
        }

        public readonly View<T> View;
        
        /// <summary>
        /// Gets the object that the View displays
        /// </summary>
        public T Item => View?.Target;
    }
    
    /// <summary>
    /// Displays a collection of items
    /// </summary>
    /// <typeparam name="T">The type of item to display</typeparam>
    public abstract class CollectionView<T> : View<IList<T>> where T : class
    {
        public event ViewEventHandler<CollectionView<T>, CollectionViewEventArgs<T>> ViewClicked;
        public event ViewEventHandler<CollectionView<T>, CollectionViewEventArgs<T>> HoveredViewChanged;
        public event ViewEventHandler<CollectionView<T>, CollectionViewEventArgs<T>> PressedViewChanged;

        public View<T> HoveredView { get; private set; }
        public View<T> PressedView { get; private set; }

        public T HoveredItem => HoveredView?.Target;
        public T PressedItem => PressedView?.Target;

        
        protected abstract IList<View<T>> Views { get; }

        protected override void ExposeProperties() { }

        protected override void OnInitialize()
        {
            foreach (var view in Views)
            {
                view.Clicked += ItemViewOnClicked;
                view.IsHoveredChanged += ItemViewOnIsHoveredChanged;
                view.IsPressedChanged += ItemViewOnIsPressedChanged;
            }
        }

        protected override void OnDispose()
        {
            foreach (var view in Views)
            {
                view.Clicked -= ItemViewOnClicked;
                view.IsHoveredChanged -= ItemViewOnIsHoveredChanged;
                view.IsPressedChanged -= ItemViewOnIsPressedChanged;
            }
        }

        protected override void OnDisplay(IList<T> target)
        {
            var prevHoveredItem = HoveredItem;
            var prevPressedItem = PressedItem;
            
            for (var i = 0; i < Views.Count; i++)
            {
                var view = Views[i];

                if(i < target.Count)
                    view.Display(target[i]);
                else view.Clear();
            }
            
            if(HoveredItem != prevHoveredItem)
                HoveredViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(HoveredView));
            
            if(PressedItem != prevPressedItem)
                PressedViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(PressedView));
        }

        private void ItemViewOnIsHoveredChanged(object view, bool isHovered)
        {
            var entry = (View<T>) view;
            if (isHovered)
            {
                if (HoveredView == entry) return;
                HoveredView = entry;
                HoveredViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(HoveredView));
            }
            else
            {
                if (HoveredView != entry) return;
                HoveredView = null;
                HoveredViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(HoveredView));
            }
        }
        
        private void ItemViewOnIsPressedChanged(object view, bool isPressed)
        {
            var entry = (View<T>) view;
            if (isPressed)
            {
                if (PressedView == entry) return;
                PressedView = entry;
                PressedViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(PressedView));
            }
            else
            {
                if (PressedView != entry) return;
                PressedView = null;
                PressedViewChanged?.Invoke(this, new CollectionViewEventArgs<T>(PressedView));
            }
        }

        private void ItemViewOnClicked(object view, ClickedEventArgs info)
        {
            var entry = (View<T>) view;
            ViewClicked?.Invoke(this, new CollectionViewEventArgs<T>(entry));
        }
    }

    /// <summary>
    /// Displays a collection of items using a particular type of views
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TView"></typeparam>
    public abstract class CollectionView<T, TView> : CollectionView<T>
        where T : class
        where TView : View<T>
    {
        [SerializeField] private List<View<T>> _views = default;
        protected override IList<View<T>> Views => _views;
        
        private void Reset()
        {
            FindChildViews();
        }

        private void FindChildViews()
        {
            _views = GetComponentsInChildren<View<T>>().ToList();
        }
    }
}