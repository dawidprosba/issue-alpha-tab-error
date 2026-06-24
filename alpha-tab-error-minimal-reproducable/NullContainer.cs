using AlphaTab;
using AlphaTab.Platform;
using AlphaTabPrototype;
namespace alpha_tab_error_minimal_reproducable
{
    internal class NullContainer : IContainer
    {
        public double Width { get; set; }
        public double Height { get; set; }
        public bool IsVisible { get; set; } = true;
        public double ScrollLeft { get; set; }
        public double ScrollTop { get; set; }

        public IEventEmitter Resize { get; set; } = new StubEventEmitter();
        public IEventEmitterOfT<IMouseEventArgs> MouseDown { get; set; } = new StubEventEmitterOfT<IMouseEventArgs>();
        public IEventEmitterOfT<IMouseEventArgs> MouseMove { get; set; } = new StubEventEmitterOfT<IMouseEventArgs>();
        public IEventEmitterOfT<IMouseEventArgs> MouseUp { get; set; } = new StubEventEmitterOfT<IMouseEventArgs>();

        public void AppendChild(IContainer child) { }
        public void StopAnimation() { }
        public void SetBounds(double x, double y, double w, double h) { Width = w; Height = h; }
        public void TransitionToX(double duration, double x) { }
        public void Clear() { }
    }
}
