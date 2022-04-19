using UnityEngine;

namespace Wosylus.UniView.Views
{
    public class CascadingView : View<Cascade>
    {
        protected override void ExposeProperties()
        {
            Expose("Cascade", cascade => cascade);
            Expose("String", cascade => cascade.SomeString);
            Expose("Color", cascade => cascade.Color);
            Expose("Alpha", cascade => cascade.Alpha);
        }
    }

    public class Cascade
    {
        public string SomeString;
        public Color Color;
        public float Alpha;
    }
}