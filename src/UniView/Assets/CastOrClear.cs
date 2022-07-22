using System;
using Wosylus.UniView;
using Wosylus.UniView.Binding.Content.Processors;

public class CastOrClear : ViewContentProcessor
{
    private ViewElementBase _owner;
    
    public override void Init(ViewElementBase owner)
    {
        base.Init(owner);
        _owner = owner;
    }

    public override bool CanProcess(Type inputType)
    {
        return true;
    }

    public override Type GetOutputType(Type inputType)
    {
        //TODO: We actually need to return type of the consumer content
        return inputType;
    }

    public override void Process<T>(T content, IContentProcess process)
    {
        if(_owner.CanConsume(typeof(T)))
            process.ContinueWith(content);
        else
        {
            _owner.Clear();
            process.EndedWith(this);
        }
    }
}