using System;
using System.Collections.Generic;
using System.IO;
using AlphaTab;
using AlphaTab.Core.EcmaScript;
using AlphaTab.Importer;
using AlphaTab.Model;
using AlphaTab.Platform;
using AlphaTab.Rendering;
using AlphaTab.Synth;
using Color = AlphaTab.Model.Color;
using Font = AlphaTab.Model.Font;
using AlphaTabBounds = AlphaTab.Rendering.Utils.Bounds;
using Environment = AlphaTab.Environment;

AlphaTab.Logger.Log = new ConsoleLogger();
Environment.RenderEngines.Set("null", new RenderEngineFactory(false, () => new NullCanvas()));
Environment.PrintEnvironmentInfo(true);
var settings = new Settings();
settings.Core.Engine = "null";
settings.Core.UseWorkers = false;
settings.Core.EnableLazyLoading = false;
settings.Core.LogLevel = LogLevel.Debug;

var facade = new MinimalFacade(width: 1200);
var api = new AlphaTabApiBase<Settings>(facade, settings);
api.Renderer.PostRenderFinished.On(() => Console.WriteLine("Render complete"));
api.Error.On(e => Console.WriteLine($"Error: {e.Message}"));

var score = ScoreLoader.LoadScoreFromBytes(
    new Uint8Array(File.ReadAllBytes(Path.Combine(AppContext.BaseDirectory, "test.xml"))),
    settings);
api.Load(score);
facade.FlushInvokeQueue();

// ── NullCanvas (no-op) ────────────────────────────────────────────────────────

class NullCanvas : ICanvas
{
    public Settings Settings { get; set; } = new Settings();
    public Color Color { get; set; } = new Color(0, 0, 0, 255);
    public double LineWidth { get; set; }
    public Font Font { get; set; } = new Font("Arial", 12);
    public TextAlign TextAlign { get; set; }
    public TextBaseline TextBaseline { get; set; }
    public void BeginRender(double w, double h) { }
    public object EndRender() => null;
    public object OnRenderFinished() => null;
    public void FillText(string text, double x, double y) { }
    public MeasuredText MeasureText(string text) => new MeasuredText(text.Length * Font.Size * 0.6, Font.Size);
    public void FillMusicFontSymbol(double x, double y, double s, MusicFontSymbol sym, bool? c = false) { }
    public void FillMusicFontSymbols(double x, double y, double s, IList<MusicFontSymbol> syms, bool? c = false) { }
    public void BeginGroup(string id) { } public void EndGroup() { }
    public void BeginRotate(double cx, double cy, double a) { } public void EndRotate() { }
    public void BeginPath() { } public void ClosePath() { }
    public void Fill() { } public void Stroke() { }
    public void MoveTo(double x, double y) { } public void LineTo(double x, double y) { }
    public void FillRect(double x, double y, double w, double h) { }
    public void StrokeRect(double x, double y, double w, double h) { }
    public void FillCircle(double x, double y, double r) { }
    public void StrokeCircle(double x, double y, double r) { }
    public void BezierCurveTo(double c1x, double c1y, double c2x, double c2y, double x, double y) { }
    public void QuadraticCurveTo(double cx, double cy, double x, double y) { }
    public void Destroy() { }
}

// ── Console logger ────────────────────────────────────────────────────────────

class ConsoleLogger : AlphaTab.ILogger
{
    public void Debug(string tag, string message, params object?[] args) => Console.WriteLine($"[DBG] {tag}: {message}");
    public void Info(string tag, string message, params object?[] args)  => Console.WriteLine($"[INF] {tag}: {message}");
    public void Warning(string tag, string message, params object?[] args) => Console.WriteLine($"[WRN] {tag}: {message}");
    public void Error(string tag, string message, params object?[] args) => Console.WriteLine($"[ERR] {tag}: {message}");
}

// ── Minimal IUiFacade plumbing ────────────────────────────────────────────────

class StubEvent : IEventEmitter
{
    public Action On(Action value) => () => { };
    public void Off(Action value) { }
}

class StubEventOfT<T> : IEventEmitterOfT<T>
{
    public Action On(Action<T> value) => () => { };
    public void Off(Action<T> value) { }
    public Action On(Action value) => () => { };
    public void Off(Action value) { }
}

class NullContainer : IContainer
{
    public double Width { get; set; }
    public double Height { get; set; }
    public bool IsVisible { get; set; } = true;
    public double ScrollLeft { get; set; }
    public double ScrollTop { get; set; }
    public IEventEmitter Resize { get; set; } = new StubEvent();
    public IEventEmitterOfT<IMouseEventArgs> MouseDown { get; set; } = new StubEventOfT<IMouseEventArgs>();
    public IEventEmitterOfT<IMouseEventArgs> MouseMove { get; set; } = new StubEventOfT<IMouseEventArgs>();
    public IEventEmitterOfT<IMouseEventArgs> MouseUp { get; set; } = new StubEventOfT<IMouseEventArgs>();
    public void AppendChild(IContainer child) { }
    public void StopAnimation() { }
    public void SetBounds(double x, double y, double w, double h) { Width = w; Height = h; }
    public void TransitionToX(double duration, double x) { }
    public void Clear() { }
}

class MinimalFacade : IUiFacade<Settings>
{
    private AlphaTabApiBase<Settings> _api;
    private readonly NullContainer _root;
    private readonly Queue<Action> _queue = new Queue<Action>();
    private bool _initialized;
    private bool _flushing;

    public MinimalFacade(double width) => _root = new NullContainer { Width = width, IsVisible = true };

    public IContainer RootContainer => _root;
    public bool AreWorkersSupported => false;
    public bool CanRender => true;
    public double ResizeThrottle => 10;
    public IEventEmitter CanRenderChanged { get; } = new StubEvent();
    public IEventEmitter RootContainerBecameVisible { get; } = new StubEvent();

    public void Initialize(AlphaTabApiBase<Settings> api, Settings settings)
    {
        _api = api;
        api.Settings = settings;
    }

    public void InitialRender()
    {
        _api.Renderer.Width = (int)_root.Width;
        _api.Renderer.UpdateSettings(_api.Settings);
        _initialized = true;
        FlushInvokeQueue();
    }

    public void BeginInvoke(Action action)
    {
        _queue.Enqueue(action);
        if (_initialized) FlushInvokeQueue();
    }

    public void FlushInvokeQueue()
    {
        if (_flushing) return;
        _flushing = true;
        while (_queue.Count > 0) _queue.Dequeue()();
        _flushing = false;
    }

    public IContainer CreateCanvasElement() => new NullContainer();
    public void BeginAppendRenderResults(RenderFinishedEventArgs r) { }
    public void BeginUpdateRenderResults(RenderFinishedEventArgs r) { }
    public Action Throttle(Action action, double delay) => action;

    public bool Load(object data, Action<Score> success, Action<Exception> error)
    {
        switch (data)
        {
            case Score score:
                _queue.Enqueue(() => success(score));
                return true;
            case byte[] b:
                var loaded = ScoreLoader.LoadScoreFromBytes(new Uint8Array(b), _api.Settings);
                _queue.Enqueue(() => success(loaded));
                return true;
            default:
                return false;
        }
    }

    public bool LoadSoundFont(object data, bool append) => false;
    public IScoreRenderer CreateWorkerRenderer() => throw new NotSupportedException();
    public IAudioExporterWorker CreateWorkerAudioExporter(IAlphaSynth synth) => throw new NotSupportedException();
    public IAlphaSynth CreateWorkerPlayer() => null;
    public IAlphaSynth CreateBackingTrackPlayer() => null;
    public Cursors CreateCursors() => null;
    public IContainer CreateSelectionElement() => null;
    public IContainer GetScrollContainer() => _root;
    public AlphaTabBounds GetOffset(IContainer scrollElement, IContainer container) => new AlphaTabBounds();
    public void Destroy() { }
    public void TriggerEvent(IContainer container, string eventName, object details = null, IMouseEventArgs originalEvent = null) { }
    public void StopScrolling(IContainer scrollElement) { }
    public void SetCanvasOverflow(IContainer canvasElement, double overflow, bool isVertical) { }
    public void RemoveHighlights() { }
    public void HighlightElements(string groupId, double masterBarIndex) { }
    public void DestroyCursors() { }
    public void ScrollToY(IContainer scrollElement, double offset, double speed) { }
    public void ScrollToX(IContainer scrollElement, double offset, double speed) { }
}