using TMPro;
using UnityEngine;

namespace Wosylus.UniView.Elements
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public sealed class TextElement : ViewElement<string>
    {
        [SerializeField] private TextMeshProUGUI _textRenderer = default;

        protected override void OnDisplay(string target)
        {
            _textRenderer.enabled = true;
            _textRenderer.text = target;
        }

        protected override void OnClear()
        {
            _textRenderer.enabled = false;
            _textRenderer.text = "";
        }

        private void Reset()
        {
            _textRenderer = GetComponent<TextMeshProUGUI>();
        }
    }
}