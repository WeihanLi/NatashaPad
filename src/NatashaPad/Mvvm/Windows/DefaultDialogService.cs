// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using System.Threading.Tasks;

namespace NatashaPad.Mvvm.Windows;

internal class DefaultDialogService : IDialogService, IWindowService
{
    private readonly Window window;

    public DefaultDialogService(Window window)
    {
        this.window = window;
    }

    public Window Window => window;

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

    public async Task ShowDialogAsync(Window? owner = null)
    {
        owner ??= GetDefaultOwner();

        if (owner is null)
        {
            window.Show();
            return;
        }

        await window.ShowDialog(owner);
    }

    private static Window? GetDefaultOwner()
    {
        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            return desktop.MainWindow;
        }

        return null;
    }
}
