using System;
using UnityEngine;
using UniView.Binding;

namespace UniView.Views
{
    public class TestContent : MonoBehaviour
    {
        public Reactive<float> Health;
        public Reactive<float> Armor;
    }
    
    public class Reactive<T> : IObservable<T>
    {
        public T Value { get; }
        
        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }

    public class SomeView : View<object>
    {
        private Reactive<float> _reactive;

        protected override void Setup(ISetup<object> setup)
        {
            setup.Content("Name", x => "").RefreshContinuously();
            setup.ReactiveContent("Health", x => _reactive);
        }
    }

    public static class UniRxExtensions
    {
        public static void RefreshFromObservable<T, TExposed>(this IContentChannelSetup<T> setup, Func<T, IObservable<TExposed>> observable)
        {
            setup.OverrideController(null); //etc
        }

        public static void ReactiveContent<T, TExposed>(this ISetup<T> setup, string key, Func<T, Reactive<TExposed>> reactive)
        {
            setup.Content(key, arg => default(TExposed)).RefreshFromObservable(reactive);
        }
    }
}