using UnityEngine;
using Wosylus.UniView.Binding.Content.Processors;

public class Multiply : ViewContentProcessor<int>
{
    [SerializeField] private int _multiplier = 1;

    protected override int Process(int input)
    {
        return input * _multiplier;
    }
}