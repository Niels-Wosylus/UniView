namespace UniViewV3
{
    public interface IDisplay<T>
    {
        bool IsDisplayingContent { get; }
        T DisplayedContent { get; }
        void Display(T content);
        void Clear();
    }
}