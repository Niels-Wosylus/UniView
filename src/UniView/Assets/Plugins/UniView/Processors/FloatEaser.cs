using UnityEngine;
using Wosylus.UniView.Binding.Content.Processors;

namespace Wosylus.UniView.Processors
{
    public class FloatEaser : ViewContentProcessor<float, int, double, bool>
    {
        [Header("Input")]
        [SerializeField] private float _inputMin = 0f;
        [SerializeField] private float _inputMax = 1f;
        
        [Header("Output")]
        [SerializeField] private float _outputMin = 0f;
        [SerializeField] private float _outputMax = 1f;

        [SerializeField] private float _speed = 1f;

        private float Output => _outputMin + _outputMax * (EasedValue - _inputMin) / (_inputMax - _inputMin);
        private float EasedValue { get; set; }
        private float TargetValue { get; set; }
        private float Speed => _speed * Time.unscaledDeltaTime;

        private void Update()
        {
            if (TargetValue > EasedValue)
            {
                EasedValue += Speed;
                if (EasedValue > TargetValue)
                    EasedValue = TargetValue;
            }
            else if (TargetValue < EasedValue)
            {
                EasedValue -= Speed;
                if (EasedValue < TargetValue)
                    EasedValue = TargetValue;
            }
        }

        protected override float Process(float input)
        {
            TargetValue = input;
            return EasedValue;
        }

        protected override float Process(int input)
        {
            TargetValue = input;
            return EasedValue;
        }

        protected override float Process(bool input)
        {
            TargetValue =  input ? _inputMax : _inputMin;
            return EasedValue;
        }

        protected override float Process(double input)
        {
            TargetValue = (float)input;
            return EasedValue;
        }
    }
}