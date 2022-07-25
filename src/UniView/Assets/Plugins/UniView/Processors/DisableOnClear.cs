namespace Wosylus.UniView.Processors
{
    public class DisableOnClear : ViewExtender
    {
        public override void OnDisplay<T>(T content)
        {
            gameObject.SetActive(true);
        }

        public override void OnClear<T>(T content)
        {
            gameObject.SetActive(false);
        }
    }
}