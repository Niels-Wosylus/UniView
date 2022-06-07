﻿using System;

namespace UniView
{
    public abstract class ViewElement<T> : ViewElementBase, IDisplay<T>
    {
        public override void Consume<TContent>(TContent content)
        {
            if(content is T match)
                Display(match);
        }

        public override bool CanConsume(Type contentType)
        {
            return typeof(T).IsAssignableFrom(contentType);
        }

        public abstract void Display(T content);
    }

    public abstract class ViewElement<T, T1> : ViewElement<T>, IDisplay<T1>
    {
        public override void Consume<TContent>(TContent content)
        {
            switch (content)
            {
                case T match:
                    Display(match);
                    break;
                case T1 match1:
                    Display(match1);
                    break;
            }
        }
        
        public override bool CanConsume(Type contentType)
        {
            return typeof(T).IsAssignableFrom(contentType)
                || typeof(T1).IsAssignableFrom(contentType);
        }
        
        public abstract void Display(T1 content);
    }
}