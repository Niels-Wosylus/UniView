using UnityEngine;
using UnityEngine.UI;

namespace UniView.Views
{
    [RequireComponent(typeof(Text))]
    public class TestElement : ViewElement<string, int>
    {
        [SerializeField] private Text _textRenderer = default;
        
        public override void Display(string content)
        {
            _textRenderer.text = content;
        }

        public override void Display(int content)
        {
            _textRenderer.text = content.ToString();
        }

        public override void Clear()
        {
            _textRenderer.text = "";
        }

        private void Reset()
        {
            _textRenderer = GetComponent<Text>();
        }
    }
}