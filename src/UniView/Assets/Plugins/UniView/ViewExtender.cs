using UnityEngine;

namespace Wosylus.UniView
{
    public interface IViewExtender
    {
        void OnDisplay<T>(T content);
        void OnClear<T>(T content);
    }
    
    public abstract class ViewExtender : MonoBehaviour, IViewExtender
    {
        public abstract void OnDisplay<T>(T content);
        public abstract void OnClear<T>(T content);
    }
}