﻿using UnityEngine;
using UnityEngine.UI;
using Wosylus.UniView;

[RequireComponent(typeof(Text))]
public class TestElement : ViewElement<string, int>
{
    protected override string InspectorPrefix => "Text";
    
    [SerializeField] private Text _textRenderer = default;
    private int? _displayedInt;
        
    public override void Display(string content)
    {
        _textRenderer.text = content;
    }

    protected override string Convert(int content)
    {
        if (_displayedInt == content)
            return _textRenderer.text;

        _displayedInt = content;
        return content.ToString();
    }

    public override void Clear()
    {
        _textRenderer.text = "";
        _displayedInt = null;
    }

    private void Reset()
    {
        _textRenderer = GetComponent<Text>();
    }
}