namespace Wosylus.UniView
{
    public interface IDisplay<T>
    {
        // T DisplayedContent { get; }
        // bool IsDisplayingContent { get; }   
        void Display(T content);
        void Clear();
    }
}