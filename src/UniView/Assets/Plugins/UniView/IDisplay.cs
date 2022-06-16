namespace Wosylus.UniView
{
    // public interface IDisplayInfo<out T>
    // {
    //     T DisplayedContent { get; }
    //     bool IsDisplayingContent { get; }   
    // }
    
    public interface IDisplay<in T>// : IDisplayInfo<T>
    {
        void Display(T content);
        void Clear();
    }
}