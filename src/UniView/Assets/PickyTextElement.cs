using UnityEngine;
using UnityEngine.UI;
using Wosylus.UniView;

[RequireComponent(typeof(Text))]
public class PickyTextElement : ViewElement<string>
{
    protected override string InspectorPrefix => "Text";
    
    [SerializeField] private Text _textRenderer = default;

    public override void Display(string content)
    {
        _textRenderer.text = content;
    }
    
    public override void Clear()
    {
        _textRenderer.text = "";
    }

    private void Reset()
    {
        _textRenderer = GetComponent<Text>();
    }
}