namespace Wosylus.UniView
{
    public delegate void IsHoveredChangedHandler(object sender, bool isHovered);

    public delegate void IsPressedChangedHandler(object sender, bool isPressed);

    public delegate void ClickedHandler(object sender, ClickedEventArgs info);

    public delegate void ViewEventHandler<in TV, in TA>(TV sender, TA info);

    public struct ClickedEventArgs { }
}