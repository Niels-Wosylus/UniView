using Wosylus.UniView.Binding.Content.Processors;

public class NumberToText : ViewContentProcessor<int, float, string>
{
    protected override string Process(string input)
    {
        return input;
    }

    protected override string Process(float input)
    {
        return input.ToString("0.00");
    }

    protected override string Process(int input)
    {
        return input.ToString();
    }
}