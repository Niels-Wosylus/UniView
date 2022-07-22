namespace Wosylus.UniView.Easers
{
    /*
    public class FloatEaser : View<float, int, bool>
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
        private float TargetValue => DisplayedContent;
        private float Speed => _speed * Time.unscaledDeltaTime;
        
        protected override void Setup(ISetup<float> setup)
        {
            setup.Content("Value", _ => Output).Continuously();
        }

        protected override float Convert(int content)
        {
            return content;
        }

        protected override float Convert(bool content)
        {
            return content ? _inputMax : _inputMin;
        }

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
    }*/
}