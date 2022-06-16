using UnityEngine;
using UnityEngine.UI;

namespace Wosylus.UniView.Elements
{
    [RequireComponent(typeof(Image))]
    public sealed class ImageElement : ViewElement<Sprite>
    {
        [SerializeField] private Image _imageRenderer = default;

        public override void Display(Sprite content)
        {
            _imageRenderer.enabled = true;
            _imageRenderer.sprite = content;
        }

        public override void Clear()
        {
            _imageRenderer.enabled = false;
            _imageRenderer.sprite = null;
        }
        
        private void Reset()
        {
            _imageRenderer = GetComponent<Image>();
        }
    }
}