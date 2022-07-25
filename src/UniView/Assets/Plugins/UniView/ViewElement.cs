using System;
using Wosylus.UniView.Binding.Content.Processors;

namespace Wosylus.UniView
{
    public abstract class ViewElement<T> : ViewElementBase, IDisplay<T>
    {
        protected override string InspectorPrefix => "";

        public abstract void Display(T content);

        protected override IContentProcessor BuildContentProcessorFinalStep() => new FinalStep(this);
        
        private class FinalStep : IContentProcessor
        {
            private readonly ViewElement<T> _owner;

            public FinalStep(ViewElement<T> owner)
            {
                _owner = owner;
            }

            public Type GetOutputType(Type inputType) => null;
            
            public bool CanProcess(Type inputType)
            {
                return typeof(T).IsAssignableFrom(inputType);
            }

            public void Process<TIn>(TIn content, IContentProcess process)
            {
                switch (content)
                {
                    case T match:
                        _owner.Display(match);
                        break;
                    
                    default:
                        _owner.Clear();
                        break;
                }
                
                process.EndedWith(this);
            }
        }
    }

    public abstract class ViewElement<T, T1> : ViewElement<T>, IDisplay<T1>
    {
        public void Display(T1 content)
        {
            var converted = Convert(content);
            Display(converted);
        }

        protected abstract T Convert(T1 content);

        protected override IContentProcessor BuildContentProcessorFinalStep() => new FinalStep(this);
        
        private class FinalStep : IContentProcessor
        {
            private readonly ViewElement<T, T1> _owner;

            public FinalStep(ViewElement<T, T1> owner)
            {
                _owner = owner;
            }

            public Type GetOutputType(Type inputType) => null;
            
            public bool CanProcess(Type inputType)
            {
                return typeof(T).IsAssignableFrom(inputType)
                       || typeof(T1).IsAssignableFrom(inputType);
            }

            public void Process<TIn>(TIn content, IContentProcess process)
            {
                switch (content)
                {
                    case T match:
                        _owner.Display(match);
                        break;
                    
                    case T1 match1:
                        _owner.Display(match1);
                        break;
                    
                    default:
                        _owner.Clear();
                        break;
                }
                
                process.EndedWith(this);
            }
        }
    }
    
    public abstract class ViewElement<T, T1, T2> : ViewElement<T, T1>, IDisplay<T2>
    {
        public void Display(T2 content)
        {
            var converted = Convert(content);
            Display(converted);
        }
        
        protected abstract T Convert(T2 content);

        protected override IContentProcessor BuildContentProcessorFinalStep() => new FinalStep(this);

        private class FinalStep : IContentProcessor
        {
            private readonly ViewElement<T, T1, T2> _owner;

            public FinalStep(ViewElement<T, T1, T2> owner)
            {
                _owner = owner;
            }

            public Type GetOutputType(Type inputType) => null;

            public bool CanProcess(Type inputType)
            {
                return typeof(T).IsAssignableFrom(inputType)
                       || typeof(T1).IsAssignableFrom(inputType)
                       || typeof(T2).IsAssignableFrom(inputType);
            }

            public void Process<TIn>(TIn content, IContentProcess process)
            {
                switch (content)
                {
                    case T match:
                        _owner.Display(match);
                        break;

                    case T1 match1:
                        _owner.Display(match1);
                        break;
                    
                    case T2 match2:
                        _owner.Display(match2);
                        break;
                    
                    default:
                        _owner.Clear();
                        break;
                }

                process.EndedWith(this);
            }
        }
    }
}