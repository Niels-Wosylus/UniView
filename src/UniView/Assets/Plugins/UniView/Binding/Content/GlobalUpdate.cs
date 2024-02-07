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

        public static IDisposable Register(IGlobalUpdateCallback callback, MonoBehaviour context)
        {
#if UNITY_EDITOR
            if (Updater == null && Application.isPlaying)
                new GameObject().AddComponent<GlobalUpdate>();
#else
            if (Updater == null)
                new GameObject().AddComponent<GlobalUpdate>();
#endif
            return new Subscription(callback, context);
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
                    var context = callback.Context;
                    var contextName = callback.ContextName;
                    Debug.LogError($"Exception in global update callback, removed callback.\n{contextName}\n{e}", context);
                    callback.Dispose();
                }
            }
        }

        private class Subscription : IDisposable
        {
            private readonly IGlobalUpdateCallback _callback;
            public readonly MonoBehaviour Context;
            public readonly string ContextName;
            private bool _isDisposed;

            public Subscription(IGlobalUpdateCallback callback, MonoBehaviour context)
            {
                Callbacks.Add(this);
                _callback = callback;
                Context = context;
                ContextName = $"{context} from {context.name} in {context.gameObject.scene.name}";
            }

            public void Invoke()
            {
                if (_isDisposed)
                    return;
                
                _callback.Execute();
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