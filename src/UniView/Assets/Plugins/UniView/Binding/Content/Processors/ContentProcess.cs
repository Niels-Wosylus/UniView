using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Wosylus.UniView.Binding.Content.Processors
{
    public interface IContentProcess
    {
        void ContinueWith<T>(T content);
        void EndedWith(IContentProcessor processor);
    }
    
    public class ContentProcess : IContentProcess
    {
        public static void Run<T>(T content, IList<IContentProcessor> processors)
        {
            var process = _pool.Get();
            process._processors = processors;
            process._index = -1;
            process.ContinueWith(content);
        }
        
        private static readonly ObjectPool<ContentProcess> _pool = new ObjectPool<ContentProcess>(() => new ContentProcess());

        private ContentProcess()
        {
        }
        
        private IList<IContentProcessor> _processors;
        private int _index;
        private IContentProcessor Current => _processors[_index];

        public void ContinueWith<T>(T content)
        {
            _index++;
            if (_index >= _processors.Count)
                OnEnded();
            
            Current.Process(content, this);
        }

        public void EndedWith(IContentProcessor processor)
        {
            if (processor != Current)
                throw new Exception($"{processor} is trying to end process currently held by {Current}");
            
            OnEnded();
        }

        private void OnEnded()
        {
            _pool.Release(this);
        }
    }
}