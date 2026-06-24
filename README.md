# AlphaTab — Mono crash: "Collection was modified" during render

Minimal reproduction showing `AlphaTabApiBase` throwing on Mono (net481) with identical code that runs fine on .NET 9.

## Error

```
Error: Collection was modified; enumeration operation may not execute.
```

## Projects

| Project | Target | AlphaTab Version | Result |
|---|---|---|---|
| `mono-with-error-latest-version` | `net481` (Mono) | `1.9.0-alpha.1813` | **Throws** |
| `mono-without-error-version-when-it-was-working` | `net481` (Mono) | `1.8.0-alpha.1640` | **Works** |
| `net90-no-error` | `net9.0` | `1.9.0-alpha.1813` | **Works** |


## What the code does

Creates a headless `AlphaTabApiBase` with a no-op `IUiFacade` and `ICanvas`, loads a MusicXML file, and triggers a render synchronously via a manual `BeginInvoke` queue.

## Environment

```
[DBG] VersionInfo: alphaTab 1.9.0-alpha.1813
[DBG] VersionInfo: commit: b6031affe32bc709f352b9f7ee70e51b983a4b4c
[DBG] VersionInfo: build date: 2026-05-25T04:46:13.698Z
[DBG] VersionInfo: High DPI: 1
[DBG] VersionInfo: .net Runtime: Mono 6.14.1 (tarball Tue Apr 29 17:43:02 UTC 2025)
[DBG] VersionInfo: Process: Arm64
[DBG] VersionInfo: OS Description: Unix 25.5.0.0
[DBG] VersionInfo: OS Arch: Arm64
[DBG] VersionInfo: alphaTab 1.9.0-alpha.1813
[DBG] VersionInfo: commit: b6031affe32bc709f352b9f7ee70e51b983a4b4c
[DBG] VersionInfo: build date: 2026-05-25T04:46:13.698Z
[DBG] VersionInfo: High DPI: 1
[DBG] VersionInfo: .net Runtime: Mono 6.14.1 (tarball Tue Apr 29 17:43:02 UTC 2025)
[DBG] VersionInfo: Process: Arm64
[DBG] VersionInfo: OS Description: Unix 25.5.0.0
[DBG] VersionInfo: OS Arch: Arm64
[DBG] ScoreLoader: Loading score from 1282 bytes using 6 importers
[DBG] ScoreLoader: Importing using importer Guitar Pro 3-5
[DBG] ScoreLoader: Guitar Pro 3-5 does not support the file
[DBG] ScoreLoader: Importing using importer Guitar Pro 6
[DBG] Guitar Pro 6: Loading GPX filesystem
[DBG] ScoreLoader: Guitar Pro 6 does not support the file
[DBG] ScoreLoader: Importing using importer Guitar Pro 7-8
[DBG] Guitar Pro 7-8: Loading ZIP entries
[DBG] Guitar Pro 7-8: Zip entries loaded
[DBG] ScoreLoader: Guitar Pro 7-8 does not support the file
[DBG] ScoreLoader: Importing using importer MusicXML
[DBG] ScoreLoader: Score imported using MusicXML
[DBG] AlphaTab: Generating Midi
[DBG] Midi: Begin midi generation
[DBG] Midi: Midi generation done
[DBG] AlphaTab: Generating Midi
[DBG] Midi: Begin midi generation
[DBG] Midi: Midi generation done
[DBG] Rendering: Rendering 1 tracks
[DBG] Rendering: Track 0: Piano
[DBG] Rendering: Rendering at scale 1 with layout PageView
[DBG] ScoreLayout: Creating score info glyphs
[DBG] PageView: Layouting score info
[ERR] API: An unexpected error occurred
Error: Collection was modified; enumeration operation may not execute.
```