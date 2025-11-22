// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Threading;
using MediatR;
using NatashaPad.Mvvm.Windows;

namespace NatashaPad.ViewModels;

public sealed class CommonParam
{
    public CommonParam(IServiceProvider serviceProvider,
        IMediator mediatr,
        IWindowManager windowManager,
        Dispatcher dispatcher)
    {
        ServiceProvider = serviceProvider;
        Mediatr = mediatr;
        WindowManager = windowManager;
        Dispatcher = dispatcher;
    }

    public IServiceProvider ServiceProvider { get; }
    public IMediator Mediatr { get; }
    public IWindowManager WindowManager { get; }
    public Dispatcher Dispatcher { get; }
}
