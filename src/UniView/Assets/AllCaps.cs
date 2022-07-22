using Wosylus.UniView.Binding.Content.Processors;

public class AllCaps : ViewContentProcessor<string>
{
    protected override string Process(string input)
    {
        return input.ToUpper();
    }
}