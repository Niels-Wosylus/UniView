using System;
using UnityEngine;

namespace Wosylus.UniView.Binding.Content.Processors
{
    public interface IContentProcessor
    {
        bool CanProcess(Type inputType);
        Type GetOutputType(Type inputType);
        void Process<T>(T content, IContentProcess process);
    }
    
    public abstract class ViewContentProcessor : MonoBehaviour, IContentProcessor
    {
        public virtual void Init(ViewElementBase owner) { }
        public abstract bool CanProcess(Type inputType);
        public abstract Type GetOutputType(Type inputType);
        public abstract void Process<T>(T content, IContentProcess process);
    }
    
    public abstract class ViewContentProcessor<TOut> : ViewContentProcessor
    {
        public override Type GetOutputType(Type inputType)
        {
            return typeof(TOut);
        }
        
        public override bool CanProcess(Type inputType)
        {
            return typeof(TOut).IsAssignableFrom(inputType);
        }
        
        public override void Process<T>(T content, IContentProcess process)
        {
            if (content is not TOut input)
            {
                process.EndedWith(this);
                return;
            }
            
            var converted = Process(input);
            process.ContinueWith(converted);
        }

        protected abstract TOut Process(TOut input);
    }
    
    public abstract class ViewContentProcessor<TOut, TIn1> : ViewContentProcessor<TOut>
    {
        public override bool CanProcess(Type inputType)
        {
            return typeof(TIn1).IsAssignableFrom(inputType)
                   || base.CanProcess(inputType);
        }

        public override void Process<T>(T content, IContentProcess process)
        {
            if (content is not TIn1 match)
            {
                base.Process(content, process);
                return;
            }

            var output = Process(match);
            process.ContinueWith(output);
        }

        protected abstract TOut Process(TIn1 input);
    }
    
    public abstract class ViewContentProcessor<TOut, TIn1, TIn2> : ViewContentProcessor<TOut, TIn1>
    {
        public override bool CanProcess(Type inputType)
        {
            return typeof(TIn2).IsAssignableFrom(inputType)
                   || base.CanProcess(inputType);
        }

        public override void Process<T>(T content, IContentProcess process)
        {
            if (content is not TIn2 match)
            {
                base.Process(content, process);
                return;
            }
            
            var output = Process(match);
            process.ContinueWith(output);
        }

        protected abstract TOut Process(TIn2 input);
    }
    
    public abstract class ViewContentProcessor<TOut, TIn1, TIn2, TIn3> : ViewContentProcessor<TOut, TIn1, TIn2>
    {
        public override bool CanProcess(Type inputType)
        {
            return typeof(TIn3).IsAssignableFrom(inputType)
                   || base.CanProcess(inputType);
        }

        public override void Process<T>(T content, IContentProcess process)
        {
            if (content is not TIn3 match)
            {
                base.Process(content, process);
                return;
            }
            
            var output = Process(match);
            process.ContinueWith(output);
        }

        protected abstract TOut Process(TIn3 input);
    }
}