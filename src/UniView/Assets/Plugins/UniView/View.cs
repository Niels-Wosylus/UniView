using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using Wosylus.UniView.Exposure;

namespace Wosylus.UniView
{
    /// <summary>
    /// Displays an object to the user interface
    /// </summary>
    /// <typeparam name="T">Type of object to display</typeparam>
    public abstract class View<T> : ViewElement<T>, IPropertyExposer,
        IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private ViewMode _viewMode = default;
        internal override ViewMode ViewMode => _viewMode;
        
        /// <summary>
        /// The object that is currently being displayed
        /// </summary>
        public T Target { get; private set; }

        [FormerlySerializedAs("_viewComponents")]
        [ReadOnly]
        [SerializeField] private MonoBehaviour[] _children = Array.Empty<MonoBehaviour>();
        private bool _didInitialize;

        #region VIEW EVENTS
        /// <summary>
        /// Called when the user successfully presses and then releases the View
        /// </summary>
        public event ClickedHandler Clicked;
        
        /// <summary>
        /// Called when the pointer enters or leaves the View
        /// </summary>
        public event IsHoveredChangedHandler IsHoveredChanged;
        
        /// <summary>
        /// Called when the player presses or releases the View
        /// </summary>
        public event IsPressedChangedHandler IsPressedChanged;
        
        /// <summary>
        /// Whether the pointer is hovering on the View
        /// </summary>
        public bool IsHovered { get; private set; }
        
        /// <summary>
        /// Whether the user is pressing and holding on the View
        /// </summary>
        public bool IsPressed { get; private set; }

        public void OnPointerEnter(PointerEventData eventData)
        {
            IsHovered = true;
            IsHoveredChanged?.Invoke(this, IsHovered);   
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            IsHovered = false;
            IsHoveredChanged?.Invoke(this, IsHovered);

            if (!IsPressed) return;
            IsPressed = false;
            IsPressedChanged?.Invoke(this, IsPressed);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Clicked?.Invoke(this, new ClickedEventArgs());
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            IsPressed = true;
            IsPressedChanged?.Invoke(this, IsPressed);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            IsPressed = false;
            IsPressedChanged?.Invoke(this, IsPressed);
        }
        
        #endregion

        /// <summary>
        /// Display an object. If target is null, the Views is cleared.
        /// </summary>
        /// <param name="target">Object to display</param>
        public void Display(T target)
        {
            if (!_didInitialize)
            {
                OnInitialize();
                _didInitialize = true;
            }
            
            if (target == null && Target != null)
            {
                Clear();
            }
            else
            {
                Target = target;
                Refresh();
            }
        }

        /// <summary>
        /// Ask the View to display nothing
        /// </summary>
        public void Clear()
        {
            if (Target == null) return;
            OnClear(Target);
            Target = default;
            ClearElements();
        }

        /// <summary>
        /// Refreshes the View
        /// </summary>
        public void Refresh()
        {
            if (Target == null) return;
            OnClear(Target);
            OnDisplay(Target);
            UpdateElements();
        }

        protected override void OnDisplay(T target) { }
        protected override void OnClear() { }
        protected virtual void OnClear(T target) { }
        
        private void UpdateElements()
        {
            InitExposureIfRequired();
            foreach (var vc in _children)
            {
                ((IPropertyConsumer) vc).ConsumeFrom(this);
            }
        }

        private void ClearElements()
        {
            foreach (var vc in _children)
            {
                ((IPropertyConsumer) vc).Clear();
            }
        }
        
        #region LIFECYCLE
        
        /// <summary>
        /// Called when the view is created (Start)
        /// </summary>
        protected virtual void OnInitialize() { }

        /// <summary>
        /// Called when the view is destroyed (OnDestroy)
        /// </summary>
        protected virtual void OnDispose() { }

        private void Start()
        {
            if (_didInitialize) return;
            OnInitialize();
            _didInitialize = true;
        }
        
        private void OnDestroy()
        {
            if(_didInitialize)
                OnDispose();
        }
        
        #endregion

        #region PROPERTY EXPOSURE
        private Dictionary<Type, IExposer<T>> _exposersByType;
        private Dictionary<string, IExposer<T>> _exposersByKey;

        /// <summary>
        /// Use this method to expose values to ViewComponents by calling Expose. Example:
        /// <para>Expose("Name", target => target.Name)</para>
        /// </summary>
        protected abstract void ExposeProperties();
        
        /// <summary>
        /// Exposes a value to ViewComponents
        /// <para>NOTE: Should only be called in override of ExposeProperties method</para>
        /// <para>Example: Expose("Name", target => target.Name)</para>
        /// </summary>
        /// <param name="key">Key to access value by</param>
        /// <param name="function">Function that returns the exposed value</param>
        /// <typeparam name="TOutput">Type of the exposed value</typeparam>
        protected void Expose<TOutput>(string key, Func<T, TOutput> function)
        {
            var outputType = typeof(TOutput);
            if (!_exposersByType.ContainsKey(outputType))
                _exposersByType.Add(outputType, new Exposer<T, TOutput>());

            var exposer = _exposersByType[outputType];
            ((Exposer<T, TOutput>)exposer).AddExposure(key, function);
            _exposersByKey.Add(key, exposer);
        }

        TOutput IPropertyExposer.GetExposedValue<TOutput>(string key)
        {
            var outputType = typeof(TOutput);
            if(!_exposersByType.ContainsKey(outputType))
                throw new Exception($"{GetType().Name} does not directly expose values of type {outputType.Name}");
            
            var exposer = (Exposer<T, TOutput>)_exposersByType[outputType];
            
            if (!exposer.Exposes(key))
                throw new Exception($"{GetType().Name} does not directly expose values of type {outputType.Name} through key {key}");
            
            return exposer.GetValue(key, Target);
        }

        Type IPropertyExposer.GetExposedType(string key)
        {
            InitExposureIfRequired();
            return _exposersByKey[key].ExposedType;
        }

        bool IPropertyExposer.DirectlyExposesType(Type type)
        {
            InitExposureIfRequired();
            return _exposersByType.ContainsKey(type);
        }

        bool IPropertyExposer.ExposesKey(string key)
        {
            InitExposureIfRequired();
            return _exposersByKey.ContainsKey(key);
        }

        IEnumerable<string> IPropertyExposer.GetExposedKeys(Type type)
        {
            InitExposureIfRequired();
            return !_exposersByType.ContainsKey(type) ? new string[0] : _exposersByType[type].GetKeys();
        }

        private void InitExposureIfRequired()
        {
            if (_exposersByKey != null) return;
            _exposersByKey = new Dictionary<string, IExposer<T>>();
            _exposersByType = new Dictionary<Type, IExposer<T>>();
            ExposeProperties();
        }
        #endregion

        #region EDITOR
        void IPropertyExposer.UpdateChildList()
        {
            var consumers = GetComponentsInChildren<IPropertyConsumer>()
                .Where(child => !ReferenceEquals(child, this))
                .Where(child => ReferenceEquals(child.GetParent(), this))
                .Cast<MonoBehaviour>();

            _children = consumers.ToArray();
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            if(_children.Contains(null))
                ((IPropertyExposer)this).UpdateChildList();
        }
        #endregion
    }
}
