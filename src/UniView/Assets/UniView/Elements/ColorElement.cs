using System;
using UnityEngine;
using UnityEngine.UI;

namespace UniView.Elements
{
    public sealed class ColorElement : ViewElement<Color>
    {
        [SerializeField] private ColorMode _mode = default;
        [SerializeField] private Graphic[] _graphics = default;
        [SerializeField, HideInInspector] private Color[] _originalColors = default;

        private void ApplyDirect(Color target)
        {
            foreach (var graphic in _graphics)
            {
                graphic.color = target;
            }
        }
        
        private void ApplyMultiply(Color target)
        {
            for (int i = 0; i < _graphics.Length; i++)
            {
                _graphics[i].color = target * _originalColors[i];
            }
        }

        public override void Display(Color content)
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
                _graphics[i].color = _originalColors[i];
            }
        }

        protected override void OnValidate()
        {
            base.OnValidate();
            GetOriginalColors();
        }

        private void GetOriginalColors()
        {
            _originalColors = new Color[_graphics.Length];
            for (int i = 0; i < _graphics.Length; i++)
            {
                _originalColors[i] = _graphics[i].color;
            }
        }
        
        private void Reset()
        {
            var graphic = GetComponent<Graphic>();
            if (graphic != null) _graphics = new[] { graphic };
        }
    }

    public enum ColorMode
    {
        /// <summary>
        /// Ignores original value
        /// </summary>
        Direct,
        
        /// <summary>
        /// Multiplies with original value
        /// </summary>
        Multiply,
    }
}