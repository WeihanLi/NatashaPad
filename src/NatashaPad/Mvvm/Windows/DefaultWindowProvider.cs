// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;

namespace NatashaPad.Mvvm.Windows;

public class DefaultWindowProvider : IWindowProvider
{
    private readonly IViewTypeInfoLocator viewTypeInfoLocator;

    public DefaultWindowProvider(IViewTypeInfoLocator viewTypeInfoLocator)
    {
        this.viewTypeInfoLocator = viewTypeInfoLocator;
    }

    public Window Create(object view, object viewModel)
    {
        var viewInfo = viewTypeInfoLocator.GetViewInfo(view.GetType());

        if (view is not Window window)
        {
            window = new Window
            {
                Content = view
            };

            if (viewInfo != default)
            {
                if (viewInfo.Width.HasValue)
                {
                    window.Width = viewInfo.Width.Value;
                }

                if (viewInfo.Height.HasValue)
                {
                    window.Height = viewInfo.Height.Value;
                }

                if (viewInfo.SizeToContent.HasValue)
                {
                    window.SizeToContent = viewInfo.SizeToContent.Value;
                }

                window.WindowStartupLocation = viewInfo.WindowStartupLocation;
                if (!string.IsNullOrEmpty(viewInfo.Title))
                {
                    window.Title = viewInfo.Title!;
                }
            }
        }

        window.DataContext = viewModel;
        return window;
    }
}
