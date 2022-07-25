using Wosylus.UniView;
using Wosylus.UniView.Binding;

public class NoView : View<NoPhase>
{
    protected override void Setup(ISetup<NoPhase> setup)
    {
        setup.Content("Value", x => x.No);
    }
}