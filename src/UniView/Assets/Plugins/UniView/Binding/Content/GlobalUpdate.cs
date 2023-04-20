using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wosylus.UniView.Binding.Content
{
    public class GlobalUpdate : MonoBehaviour
    {
        private static GlobalUpdate Updater { get; set; }
        private static readonly List<Action> Callbacks = new List<Action>(128); 
        
        public static void Clear()
        {
            Callbacks.Clear();
        }

        public static IDisposable Register(Action callback)
        {
#if UNITY_EDITOR
            if (Updater == null && Application.isPlaying)
                new GameObject().AddComponent<GlobalUpdate>();
#else
            if (Updater == null)
                new GameObject().AddComponent<GlobalUpdate>();
#endif
            return new Subscription(callback);
        }

        private void Awake()
        {
            if(Updater != null)
                Destroy(this);
            Updater = this;
            DontDestroyOnLoad(gameObject);
            name = "Global View Updater";
        }

        private void Update()
        {
            for (var i = 0; i < Callbacks.Count; i++)
            {
                var callback = Callbacks[i];
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