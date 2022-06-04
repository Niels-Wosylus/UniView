namespace UniView.Views
{
    public class SomeView : View<object>
    {
        protected override void ExposeContents()
        {
            Expose("Name", x => x.ToString());
        }
    }
}