# PS5 PKG Tool - Cross-Platform Rewrite

A cross-platform rewrite of PS5 PKG Tool targeting Linux, macOS, and Windows using .NET 10 and Avalonia UI.

## Projects

| Project | Description |
|---------|-------------|
| `PS5PKGTool.Core` | Core library: PKG parsing, building, extraction, verification (cross-platform) |
| `PS5PKGTool.Ffpfsc` | exFAT / FFPKG / FFPFSC image handling (cross-platform) |
| `PS5PKGTool.Ufs2` | UFS2 filesystem implementation (cross-platform) |
| `PS5PKGTool.App` | Cross-platform Avalonia UI app (Windows, macOS, Linux) |

## Requirements

- .NET 10 SDK
- On Linux: `libgl1-mesa-glx` and `libgl1-mesa-dri` (for Avalonia GPU acceleration)

## Build

```bash
# Linux / macOS / Windows
dotnet build PS5PKGTool.slnx -c Release
```

The app is written to `PS5PKGTool.App/bin/Release/net10.0/`.

## Run

```bash
# Linux / macOS / Windows
dotnet run --project PS5PKGTool.App -c Release
```

## Features

- Library scanning and browsing
- Image tools (convert, extract, verify, repair)
- Package builder (debug FPKG)
- Settings

## Cross-Platform Notes

The core libraries (`Core`, `Ffpfsc`, `Ufs2`) are pure managed C# and compile identically on all platforms. The UI uses Avalonia, a cross-platform .NET UI framework that runs on Windows, macOS, and Linux.

Some Windows-only features from the original app (e.g. shell integration, Win32-specific P/Invoke) are not included in this rewrite.
