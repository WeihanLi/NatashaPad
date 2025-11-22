// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia;
using Avalonia.ReactiveUI;

namespace NatashaPad;

internal static class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
}
