# AlphaTab — Mono crash: "Collection was modified" during render

Minimal reproduction showing `AlphaTabApiBase` throwing on Mono (net481) with identical code that runs fine on .NET 9.

## Error

```
Error: Collection was modified; enumeration operation may not execute.
```

## Projects

| Project | Target | Result |
|---|---|---|
| `alpha-tab-error-minimal-reproducable` | `net481` (Mono) | **Throws** |
| `alpha-tab-no-error-same-code` | `net9.0` | **Works** |

Both projects have **identical `Program.cs`** and both reference **AlphaTab `1.9.0-alpha.1813`**.


## What the code does

Creates a headless `AlphaTabApiBase` with a no-op `IUiFacade` and `ICanvas`, loads a MusicXML file, and triggers a render synchronously via a manual `BeginInvoke` queue.

## Environment

- AlphaTab: `1.9.0-alpha.1813`
- Failing runtime: Mono / .NET Framework 4.8.1 (`net481`)
- Passing runtime: .NET 9