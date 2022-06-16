using UnityEngine;
using UnityEngine.UI;

namespace Wosylus.UniView.Elements
{
    [RequireComponent(typeof(Image))]
    public sealed class ImageFillElement : ViewElement<float>
    {
        [SerializeField] private Image _image = default;

        public override void Display(float content)
        {
            _image.fillAmount = content;
        }

        public override void Clear()
        {
            _image.fillAmount = 0;
        }
        
        private void Reset()
        {
            _image = GetComponent<Image>();
        }
    }
}