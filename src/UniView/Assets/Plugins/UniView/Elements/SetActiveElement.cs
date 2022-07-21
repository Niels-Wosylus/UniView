namespace Wosylus.UniView.Elements
{
    public class SetActiveElement : ViewElement<bool>
    {
        public override void Display(bool content)
        {
            gameObject.SetActive(content);
        }

        public override void Clear()
        {
        }
    }
}