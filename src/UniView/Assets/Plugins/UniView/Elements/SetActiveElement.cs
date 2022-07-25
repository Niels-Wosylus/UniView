using UnityEngine;

namespace Wosylus.UniView.Elements
{
    public class SetActiveElement : ViewElement<bool>
    {
        [SerializeField] private SetActiveElementMode _mode = default;

        public override void Display(bool content)
        {
            var active = _mode == SetActiveElementMode.EnableWhenTrue ? content : !content;
            gameObject.SetActive(active);
        }

        public override void Clear()
        {
            gameObject.SetActive(false);
        }
    }
    
    public enum SetActiveElementMode
    {
        EnableWhenTrue,
        DisableWhenTrue,
    }
}