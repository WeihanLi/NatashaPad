// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System.Threading.Tasks;

namespace NatashaPad.Mvvm.MessageBox;

internal class DefaultErrorMessageBoxService : IErrorMessageBoxService
{
    public void ShowError(string title, string content)
    {
        Dispatcher.UIThread.Post(() => _ = ShowAsync(title, content));
    }

    private static async Task ShowAsync(string title, string content)
    {
        var dialog = new Window
        {
            Title = title,
            SizeToContent = SizeToContent.WidthAndHeight,
            CanResize = false,
            Content = BuildContent(content)
        };

        if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop &&
            desktop.MainWindow is Window owner)
        {
            await dialog.ShowDialog(owner);
        }
        else
        {
            dialog.Show();
        }
    }

    private static Control BuildContent(string content)
    {
        var message = new TextBlock
        {
            Text = content,
            TextWrapping = TextWrapping.Wrap,
            Margin = new Thickness(0, 0, 0, 12),
            MaxWidth = 360
        };

        var okButton = new Button
        {
            Content = "OK",
            HorizontalAlignment = HorizontalAlignment.Right,
            MinWidth = 80
        };

        okButton.Click += (_, args) =>
        {
            if (okButton.GetVisualRoot() is Window owner)
            {
                owner.Close();
            }
        };

        var panel = new StackPanel
        {
            Orientation = Orientation.Vertical,
            Margin = new Thickness(24),
            Spacing = 8
        };

        panel.Children.Add(message);
        panel.Children.Add(okButton);

        return panel;
    }
}
