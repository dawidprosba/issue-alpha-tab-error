// See https://aka.ms/new-console-template for more information

using System;
using System.Collections.Generic;
using System.IO;
using alpha_tab_error_minimal_reproducable;
using AlphaTab;
using AlphaTab.Core.EcmaScript;
using AlphaTab.Importer;
using Environment = AlphaTab.Environment;


var dict = new Dictionary<int, string>
{
    { 1, "beat1" },
    { 2, "beat2" },
    { 3, "beat3" }
};

foreach (var v in dict.Values)
{
    Console.WriteLine($"Processing: {v}");
    dict.Clear(); // same as AlphaTab's bug
}

//
// // Register our canvas factory before creating the API
// Environment.RenderEngines.Set("unity",
//     new RenderEngineFactory(false, () => new DebugCanvas()));
//
// var settings = new Settings();
// settings.Core.Engine = "unity";
// settings.Core.UseWorkers = false;
//             
// // Disable lazy loading so partials render directly inside layoutAndRender()
// // rather than being deferred. Our synchronous BeginInvoke can't safely defer
// // RenderResult() calls the way a real UI dispatcher would.
// settings.Core.EnableLazyLoading = false;
//
// var facade = new UnityUiFacade(width: 1200);
// var api = new AlphaTabApiBase<Settings>(facade, settings);
//
// api.Renderer.PostRenderFinished.On(() =>
//     Console.WriteLine("[AlphaTab] === All rendering complete ==="));
// api.Error.On(e =>
//     Console.WriteLine($"[ERROR][AlphaTab] Error: {e.Message}"));
//
// var path = Path.Combine(AppContext.BaseDirectory, "test.xml");
// var bytes = File.ReadAllBytes(path);
// var score = ScoreLoader.LoadScoreFromBytes(new Uint8Array(bytes), settings);
// Console.WriteLine($"[AlphaTab] Loaded: '{score.Title}' — {score.Tracks.Count} track(s)");
// api.ScoreLoaded.On(s => Console.WriteLine($"[AlphaTab] Score loaded via API"));
// api.Load(score);
// Console.WriteLine($"[Loader] After Load (before flush), queue size = {facade._invokeQueue.Count}");
// facade.FlushInvokeQueue();
// Console.WriteLine($"[Loader] After flush, queue size = {facade._invokeQueue.Count}");
