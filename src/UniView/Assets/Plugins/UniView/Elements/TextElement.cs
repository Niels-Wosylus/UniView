using TMPro;
using UnityEngine;

namespace Wosylus.UniView.Elements
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TextElement : ViewElement<string, int, float>
    {
        protected override string InspectorPrefix => "Text";
        
        [SerializeField] private TextMeshProUGUI _textRenderer = default;
        private int? _displayedInt;
        private float? _displayedFloat;

        public override void Display(string content)
        {
            _textRenderer.text = content;
        }

        public override void Display(int content)
        {
            if (_displayedInt == content)
                return;

            _displayedInt = content;
            _textRenderer.text = content.ToString();
        }

        public override void Display(float content)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (content == _displayedFloat)
                return;

            _displayedFloat = content;
            _textRenderer.text = content.ToString("0.00");
        }

        public override void Clear()
        {
            _textRenderer.text = "";
            _displayedInt = null;
            _displayedFloat = null;
        }
        
        private void Reset()
        {
            _textRenderer = GetComponent<TextMeshProUGUI>();
        }
    }
}