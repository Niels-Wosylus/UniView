using System;
using System.Collections;
using UnityEngine;
using Wosylus.UniView.Binding.Content.Processors;

public class Delay : ViewContentProcessor
{
    [SerializeField] private float _delayInSeconds = default;
    
    public override bool CanProcess(Type inputType)
    {
        return true;
    }

    public override Type GetOutputType(Type inputType)
    {
        return inputType;
    }

    public override void Process<T>(T content, IContentProcess process)
    {
        StartCoroutine(Waiting(content, process));
    }

    private IEnumerator Waiting<T>(T content, IContentProcess process)
    {
        yield return new WaitForSeconds(_delayInSeconds);
        process.ContinueWith(content);
    }
}