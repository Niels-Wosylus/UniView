using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniView.Elements
{
    public sealed class OpacityElement : ViewElement<float>
    {
        [SerializeField] private ColorMode _mode = default;
        [SerializeField] private Graphic[] _graphics = default;
        [SerializeField, HideInInspector] private float[] _originalAlphas = default;

        public override void Display(float content)
        {
            switch (_mode)
            {
                case ColorMode.Direct:
                    ApplyDirect(content);
                    break;
                
                case ColorMode.Multiply:
                    ApplyMultiply(content);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override void Clear()
        {
            for (int i = 0; i < _graphics.Length; i++)
            {
                var graphic = _graphics[i];
                graphic.color = BuildColor(graphic.color, _originalAlphas[i]);
            }
        }

        private void ApplyDirect(float target)
        {
            foreach (var graphic in _graphics)
            {
                graphic.color = BuildColor(graphic.color, target);
            }
        }
        
        private void ApplyMultiply(float target)
        {
            for (int i = 0; i < _graphics.Length; i++)
            {
                var graphic = _graphics[i];
                graphic.color = BuildColor(graphic.color, target * _originalAlphas[i]);
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            GetOriginalAlphas();
        }

        private void GetOriginalAlphas()
        {
            _originalAlphas = new float[_graphics.Length];
            for (int i = 0; i < _graphics.Length; i++)
            {
                _originalAlphas[i] = _graphics[i].color.a;
            }
        }

        private void Reset()
        {
            var graphic = GetComponent<Graphic>();
            if (graphic != null) _graphics = new[] { graphic };
        }

        private Color BuildColor(Color color, float alpha) => new Color(color.r, color.g, color.b, alpha);
    }
}