using System;
using System.Collections.Generic;
using UnityEngine;

namespace Wosylus.UniView.Binding.Content
{
    public class GlobalUpdate : MonoBehaviour
    {
        private static GlobalUpdate Updater { get; set; }
        private static readonly List<Subscription> Callbacks = new List<Subscription>(128); 
        private static readonly List<Subscription> ToRemove = new List<Subscription>(128);

        /// <summary>
        /// Forces to clear all callbacks. This is a hack to fix a bug where callbacks to destroyed consumers linger.
        /// </summary>
        public static void HackReset()
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
            for (var i = 0; i < ToRemove.Count; i++)
            {
                var callback = ToRemove[i];
                Callbacks.Remove(callback);
            }
            
            ToRemove.Clear();
            
            for (var i = 0; i < Callbacks.Count; i++)
            {
                var callback = Callbacks[i];
                
                try
                {
                    callback.Invoke();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception in global update callback, removed callback.\n{e}");
                    callback.Dispose();
                }
            }
        }

        private class Subscription : IDisposable
        {
            private readonly Action _callback;
            private bool _isDisposed;

            public Subscription(Action callback)
            {
                Callbacks.Add(this);
                _callback = callback;
            }
            
            public void Invoke()
            {
                if (_isDisposed)
                    return;
                
                _callback.Invoke();
            }
            
            public void Dispose()
            {
                if (_isDisposed)
                    return;
                
                _isDisposed = true;
                ToRemove.Add(this);
            }
        }
    }
}