using System;
using System.Collections.Generic;
using AlphaTab;
using AlphaTab.Model;
using AlphaTab.Platform;
using Font = AlphaTab.Model.Font;
using Color = AlphaTab.Model.Color;

namespace alpha_tab_error_minimal_reproducable
{
    internal class DebugCanvas : ICanvas
    {
        public Settings Settings { get; set; } = new Settings();
        public Color Color { get; set; } = new Color(0, 0, 0, 255);
        public double LineWidth { get; set; } = 1;
        public Font Font { get; set; } = new Font("Arial", 12);
        public TextAlign TextAlign { get; set; }
        public TextBaseline TextBaseline { get; set; }

        public void BeginRender(double width, double height) =>
            Console.WriteLine($"[AlphaTab] BeginRender: {width}x{height}");

        public object EndRender()
        {
            Console.WriteLine("[AlphaTab] EndRender");
            return null;
        }

        public object OnRenderFinished() => null;

        public void FillText(string text, double x, double y) =>
            Console.WriteLine($"[AlphaTab] Text: '{text}' at ({x:F0},{y:F0})");

        public MeasuredText MeasureText(string text) =>
            new MeasuredText(text.Length * Font.Size * 0.6, Font.Size);

        public void FillMusicFontSymbol(double x, double y, double relativeScale,
            MusicFontSymbol symbol, bool? centerAtPosition = false) =>
            Console.WriteLine($"[AlphaTab] Symbol: {symbol} at ({x:F0},{y:F0}) scale={relativeScale:F2}");

        public void FillMusicFontSymbols(double x, double y, double relativeScale,
            IList<MusicFontSymbol> symbols, bool? centerAtPosition = false) =>
            Console.WriteLine($"[AlphaTab] Symbols[{symbols.Count}] at ({x:F0},{y:F0})");

        public void BeginGroup(string identifier) => 
            Console.WriteLine($"[Canvas] BeginGroup: {identifier}");
        public void EndGroup() { }
        public void BeginRotate(double centerX, double centerY, double angle) { }
        public void EndRotate() { }
        public void BeginPath() { }
        public void ClosePath() { }
        public void Fill() => Console.WriteLine("[Canvas] Fill");
        public void Stroke() { }
        public void MoveTo(double x, double y) =>
            Console.WriteLine($"[Canvas] MoveTo ({x:F0},{y:F0})");
        public void LineTo(double x, double y) { }
        public void FillRect(double x, double y, double w, double h) =>
            Console.WriteLine($"[Canvas] FillRect ({x:F0},{y:F0}) {w:F0}x{h:F0}");
        public void StrokeRect(double x, double y, double w, double h) { }
        public void FillCircle(double x, double y, double radius) { }
        public void StrokeCircle(double x, double y, double radius) { }
        public void BezierCurveTo(double cp1X, double cp1Y, double cp2X, double cp2Y, double x, double y) { }
        public void QuadraticCurveTo(double cpx, double cpy, double x, double y) { }
        public void Destroy() { }
    }
}
