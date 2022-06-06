using System;
using UnityEngine;
using UniView.Exposure;

namespace UniView.Views
{
    public class Observable<T> : IObservable<T>
    {
        public IDisposable Subscribe(IObserver<T> observer)
        {
            throw new NotImplementedException();
        }
    }

    public class SomeView : View<MonoBehaviour>
    {
        private Observable<float> _observable;

        protected override void Setup(ISetup<MonoBehaviour> setup)
        {
            setup.Content("Name", x => x.name).RefreshContinuously();
            setup.Content("Health", _observable);
        }
    }

    public static class UniRxExtensions
    {
        public static void RefreshFromObservable<T, TExposed>(this IContentChannelSetup<T> setup, IObservable<TExposed> observable)
        {
            setup.OverrideController(null); //etc
        }

        public static void Content<T, TExposed>(this ISetup<T> setup, string key, IObservable<TExposed> observable) //<--- should be ReactiveProperty
        {
            setup.Content(key, arg => default(TExposed)).RefreshFromObservable(observable); //replace func with property getter
        }
    }
}