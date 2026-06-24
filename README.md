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

- AlphaTab: `1.9.0-alpha.1813`
- Failing runtime: Mono / .NET Framework 4.8.1 (`net481`)
- Passing runtime: .NET 9