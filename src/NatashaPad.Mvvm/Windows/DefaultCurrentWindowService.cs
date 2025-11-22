// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;
using System.Threading.Tasks;

namespace NatashaPad.Mvvm.Windows;

internal class DefaultCurrentWindowService : ICurrentWindowService
{
    private readonly Window window;

    public DefaultCurrentWindowService(Window window)
    {
        this.window = window;
    }

    public void Close()
    {
        window.Close();
    }

    public void Hide()
    {
        window.Hide();
    }

    public void Show()
    {
        window.Show();
    }

    public Window Window => window;

    public Task ShowDialogAsync(Window? owner = null)
    {
        // Current window is already visible; no dialog to show.
        return Task.CompletedTask;
    }
}
