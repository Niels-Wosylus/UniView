using System;
using System.Collections.Generic;
using UnityEngine;

namespace UniView.Exposure
{
    public class GlobalViewUpdater : MonoBehaviour
    {
        private static GlobalViewUpdater Updater { get; set; }
        private static readonly List<Action> Callbacks = new List<Action>(128); 

        public static IDisposable Register(Action callback)
        {
            #if UNITY_EDITOR
            if (Updater == null && Application.isPlaying)
                new GameObject().AddComponent<GlobalViewUpdater>();
            
            return new Subscription(callback);
            #endif
            
            if (Updater == null)
                new GameObject().AddComponent<GlobalViewUpdater>();
            
            return new Subscription(callback);
        }

        private void Awake()
        {
            if(Updater != null)
                Destroy(this);
            Updater = this;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            foreach (var callback in Callbacks)
            {
                callback.Invoke();
            }
        }

        private class Subscription : IDisposable
        {
            private readonly Action _callback;
            private bool _isDisposed;

            public Subscription(Action callback)
            {
                Callbacks.Add(callback);
                _callback = callback;
            }
            
            public void Dispose()
            {
                if (_isDisposed)
                    return;
                
                Callbacks.Remove(_callback);
            }
        }
    }
}