using Wosylus.UniView;
using Wosylus.UniView.Binding;

public class YesView : View<YesPhase>
{
    protected override void Setup(ISetup<YesPhase> setup)
    {
        setup.Content("Value", x => x.Yes);
    }
}