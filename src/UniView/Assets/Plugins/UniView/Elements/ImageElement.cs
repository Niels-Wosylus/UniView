using UnityEngine;
using UnityEngine.UI;

namespace Wosylus.UniView.Elements
{
    [RequireComponent(typeof(Image))]
    public sealed class ImageElement : ViewElement<Sprite>
    {
        [SerializeField] private Image _imageRenderer = default;

        protected override void OnDisplay(Sprite target)
        {
            _imageRenderer.enabled = true;
            _imageRenderer.sprite = target;
        }

        protected override void OnClear()
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