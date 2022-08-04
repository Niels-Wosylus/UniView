using System;
using UniViewV3.Messaging;

namespace UniViewV3.Content.Input
{
    public interface IViewModelConstructor<in TA, in TB>
    {
        IProcessor<TA?> GetProcessorA();
        IProcessor<TB?> GetProcessorB();
    }

    public interface IViewModelConstructor<in TA, in TB, in TC> : IViewModelConstructor<TA?, TB?>
    {
        IProcessor<TC?> GetProcessorC();
    }
    
    public class ViewModelConstructor<TA, TB, TC, TViewModel> : IViewModelConstructor<TA?, TB?, TC?>
    {
        private readonly Func<TA?, TB?, TC?, TViewModel> _constructor;
        private readonly IProcessor<TViewModel> _modelProcessor;

        private TA? _lastInput1;
        private TB? _lastInput2;
        private TC? _lastInput3;
        
        public ViewModelConstructor(Func<TA?, TB?, TC?, TViewModel> constructor, IProcessor<TViewModel> modelProcessor)
        {
            _constructor = constructor;
            _modelProcessor = modelProcessor;
        }

        public IProcessor<TA?> GetProcessorA() => new Processor<TA?>(Process);
        public IProcessor<TB?> GetProcessorB() => new Processor<TB?>(Process);
        public IProcessor<TC?> GetProcessorC() => new Processor<TC?>(Process);

        private void Process(TA? input1)
        {
            _lastInput1 = input1;
            PassForward();
        }

        private void Process(TB? input2)
        {
            _lastInput2 = input2;
            PassForward();
        }

        private void Process(TC? input3)
        {
            _lastInput3 = input3;
            PassForward();
        }

        private void PassForward()
        {
            var model = _constructor.Invoke(_lastInput1, _lastInput2, _lastInput3);
            _modelProcessor.Process(model);
        }
    }
}