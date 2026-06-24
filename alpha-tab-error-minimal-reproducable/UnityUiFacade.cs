using System;
using System.Collections.Generic;
using System.Diagnostics;
using AlphaTab;
using AlphaTab.Core.EcmaScript;
using AlphaTab.Importer;
using AlphaTab.Model;
using AlphaTab.Platform;
using AlphaTab.Rendering;
using AlphaTab.Synth;
using AlphaTabPrototype;
using AlphaTabBounds = AlphaTab.Rendering.Utils.Bounds;

namespace alpha_tab_error_minimal_reproducable
{
    internal class UnityUiFacade : IUiFacade<Settings>
    {
        private AlphaTabApiBase<Settings> _api;
        private readonly NullContainer _rootContainer;

        public UnityUiFacade(double width)
        {
            _rootContainer = new NullContainer { Width = width, IsVisible = true };
        }

        public IContainer RootContainer => _rootContainer;
        public bool AreWorkersSupported => false;
        public bool CanRender => true;
        public double ResizeThrottle => 10;
        public IEventEmitter CanRenderChanged { get; } = new StubEventEmitter();
        public IEventEmitter RootContainerBecameVisible { get; } = new StubEventEmitter();

        public void Initialize(AlphaTabApiBase<Settings> api, Settings settings)
        {
            _api = api;
            api.Settings = settings;
        }
        private bool _initialized = false;

        public void InitialRender()
        {
            Console.WriteLine("[Facade] InitialRender");
            _api.Renderer.Width = (int)_rootContainer.Width;
            _api.Renderer.UpdateSettings(_api.Settings);
            _initialized = true;
            FlushInvokeQueue();
        }

        public IContainer CreateCanvasElement() => new NullContainer();

        public void BeginAppendRenderResults(RenderFinishedEventArgs renderResult)
        {
            Console.WriteLine("[Facade] BeginAppendRenderResults");

            if (renderResult != null)
                Console.WriteLine($"[AlphaTab] Chunk layout: {renderResult.Width:F0}x{renderResult.Height:F0}");
        }

        public void BeginUpdateRenderResults(RenderFinishedEventArgs renderResult)
        {
            Console.WriteLine("[Facade] BeginUpdateRenderResults");
        }

        public readonly Queue<Action> _invokeQueue = new();

        public void BeginInvoke(Action action)
        {
            Console.WriteLine("[Facade] BeginInvoke queued");
            _invokeQueue.Enqueue(action);
            if (_initialized)
                FlushInvokeQueue();
        }
        private bool _isFlushing = false;


        public void FlushInvokeQueue()
        {
            if (_isFlushing) return;
            _isFlushing = true;
            while (_invokeQueue.Count > 0)
                _invokeQueue.Dequeue()();
            _isFlushing = false;
        }

        public Action Throttle(Action action, double delay) => action;

        // AlphaTab aliases Error = System.Exception in its own global usings;
        // in the compiled IL the parameter type is System.Exception.
        public bool Load(object data, Action<Score> success, Action<Exception> error)
        {
            switch (data)
            {
                case Score score:
                    _invokeQueue.Enqueue(() => success(score)); // defer it
                    return true;
                case byte[] b:
                    var loaded = ScoreLoader.LoadScoreFromBytes(new Uint8Array(b), _api.Settings);
                    _invokeQueue.Enqueue(() => success(loaded)); // defer it
                    return true;
                default:
                    return false;
            }
        }

        public bool LoadSoundFont(object data, bool append) => false;

        public IScoreRenderer CreateWorkerRenderer() =>
            throw new NotSupportedException("Workers not supported in Unity prototype");

        public IAudioExporterWorker CreateWorkerAudioExporter(IAlphaSynth synth) =>
            throw new NotSupportedException("Audio export not supported in Unity prototype");

        public IAlphaSynth CreateWorkerPlayer() => null;
        public IAlphaSynth CreateBackingTrackPlayer() => null;
        public Cursors CreateCursors() => null;
        public IContainer CreateSelectionElement() => null;
        public IContainer GetScrollContainer() => _rootContainer;
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
}
