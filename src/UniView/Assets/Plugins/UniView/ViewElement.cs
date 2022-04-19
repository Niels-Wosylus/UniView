using System.Linq;
using UnityEngine;
using Wosylus.UniView.Exposure;
#if UNITY_EDITOR
#if ODIN_INSPECTOR
#endif
#endif

namespace Wosylus.UniView
{
    public abstract class ViewElement<T> : MonoBehaviour, IPropertyConsumer
    {
        #region UNITY_INSPECTOR
        #if UNITY_EDITOR
        #if ODIN_INSPECTOR

        [InfoBox("$" + nameof(GetInspectorMessage), InfoMessageType = InfoMessageType.Info),
         ShowIf(nameof(HasValidParent)), ShowInInspector, HideLabel, PropertyOrder(9999), ShowIf(nameof(IsElement))]
        private string ValidParentProp => "Valid parent view";

        [InfoBox("$" + nameof(GetInspectorMessage), InfoMessageType = InfoMessageType.Warning),
         ShowIf(nameof(HasNoParent)), ShowInInspector, HideLabel, PropertyOrder(9999), ShowIf(nameof(IsElement))]
        private string NoParentProp => "No parent view";
        
        [InfoBox("$" + nameof(GetInspectorMessage), InfoMessageType = InfoMessageType.Warning),
         ShowIf(nameof(HasInvalidParent)), ShowInInspector, HideLabel, PropertyOrder(9999), ShowIf(nameof(IsElement))]
        private string InvalidParentProp => "Invalid parent view";
        
        private string GetInspectorMessage()
        {
            var view = GetParent();
            if (view == null) return "For this component to receive data to display, place it on or under a View";

            var viewName = $"{((MonoBehaviour) view).name} ({view.GetType().Name})";
            
            if (!ViewIsValidParent(view))
                return $"{viewName} does not expose any values of type {typeof(T).Name}";

            return $"Displays {_key} ({typeof(T).Name}) from {viewName}";
        }

        private bool HasValidParent => ViewIsValidParent(GetParent());
        private bool HasNoParent => GetParent() == null;
        private bool HasInvalidParent => !HasNoParent && !HasValidParent;

        private bool ViewIsValidParent(IPropertyExposer view)
        {
            return view != null && view.DirectlyExposesType(typeof(T));
        }

        private IEnumerable<string> GetValidKeys()
        {
            return HasValidParent ? GetParent().GetExposedKeys(typeof(T)) : new[] {""};
        }

        private bool IsElement => ViewMode == ViewMode.Element;
        
        [ValueDropdown(nameof(GetValidKeys)), ShowIf(nameof(IsElement))]
        #endif
        #endif
        #endregion
        [SerializeField] private string _key = "";

        internal virtual ViewMode ViewMode => ViewMode.Element;
        private bool _didInit;

        protected abstract void OnDisplay(T target);
        protected abstract void OnClear();

        private void OnEnable()
        {
            if (!_didInit)
                OnClear();
        }
        
        void IPropertyConsumer.ConsumeFrom(IPropertyExposer exposer)
        {
            if (string.IsNullOrEmpty(_key))
                return;
                    
            if(!exposer.ExposesKey(_key))
            {
                Debug.LogWarning($"Trying to display unexposed property with key {_key}");
                return;
            }

            var exposedType = exposer.GetExposedType(_key);
            if(exposedType != typeof(T))
            {
                Debug.LogWarning($"Property with key {_key} is of type {exposedType} which is not assignable to {typeof(T)}");
                return;
            }
            
            OnDisplay(exposer.GetExposedValue<T>(_key));
            _didInit = true;
        }

        void IPropertyConsumer.Clear()
        {
            OnClear();
        }

        IPropertyExposer IPropertyConsumer.GetParent() => GetParent();

        private IPropertyExposer GetParent()
        {
            var parent = transform;                           
            while (parent != null)                            
            {                                                 
                var view = parent.GetComponent<IPropertyExposer>();
                if (view != null && !ReferenceEquals(view, this)) return view;          
                parent = parent.parent;                       
            }                                                 
                                                  
            return null; 
        }

        protected virtual void OnValidate()
        {
            if (ViewMode == ViewMode.Root)
            {
                _key = "";
                return;
            }
            
            var parent = GetParent();
            if (parent == null) return;
            
            parent.UpdateChildList();

            if (!parent.ExposesKey(_key))
                _key = "";
            else
            {
                if (!(parent.GetExposedType(_key) == typeof(T)))
                    _key = "";
            }

            if (!string.IsNullOrEmpty(_key)) return;
            
            if (parent.DirectlyExposesType(typeof(T)))
                _key = parent.GetExposedKeys(typeof(T)).First();
        }
    }

    internal enum ViewMode
    {
        Root,
        Element
    }
}