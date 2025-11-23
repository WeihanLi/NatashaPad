// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;

namespace NatashaPad.Mvvm.Windows;

internal class DefaultWindowManager : IWindowManager
{
    private readonly IViewInstanceLocator locator;
    private readonly IWindowProvider windowProvider;
    private readonly Dictionary<object, Window> windowMap;

    public DefaultWindowManager(
        IViewInstanceLocator locator,
        IWindowProvider windowProvider)
    {
        this.locator = locator;
        this.windowProvider = windowProvider;
        windowMap = new Dictionary<object, Window>();
    }

    public ICurrentWindowService GetCurrent<TViewModel>(TViewModel viewModel)
    {
        if (!windowMap.TryGetValue(viewModel!, out var window))
        {
            throw new ArgumentException("No open window is associated with the provided view model instance.", nameof(viewModel));
        }

        return new DefaultCurrentWindowService(window);
    }

    public IDialogService GetDialogService<TViewModel>(TViewModel viewModel)
    {
        var view = locator.GetView(typeof(TViewModel));
        var window = windowProvider.Create(view, viewModel!);
        window.Closed += Window_Closed;

        windowMap[viewModel!] = window;
        return new DefaultDialogService(window);
    }

    public IWindowService GetWindowService<TViewModel>(TViewModel viewModel)
    {
        return GetDialogService(viewModel);
    }

    private void Window_Closed(object? sender, EventArgs e)
    {
        if (sender is Window window)
        {
            window.Closed -= Window_Closed;
        }

        windowMap.Remove(FindViewModel(sender));
    }

    private object FindViewModel(object? window)
    {
        foreach (var pair in windowMap)
        {
            if (Equals(pair.Value, window))
            {
                return pair.Key;
            }
        }

        throw new InvalidOperationException("Unable to resolve the view model associated with the closed window.");
    }
}
