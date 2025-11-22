// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;

namespace NatashaPad.Mvvm.Windows;

internal class DefaultWindowManager : IWindowManager
{
    private readonly IViewInstanceLocator locator;
    private readonly IWindowProvider windowProvider;

    public DefaultWindowManager(
        IViewInstanceLocator locator,
        IWindowProvider windowProvider)
    {
        this.locator = locator;
        this.windowProvider = windowProvider;

        windowMap = new Dictionary<object, Window>();
    }

    private readonly Dictionary<object, Window> windowMap;

    public ICurrentWindowService GetCurrent<TViewModel>(TViewModel viewModel)
    {
        if (!windowMap.TryGetValue(viewModel, out var window))
        {
            throw new ArgumentException(Properties.Resource.FoundNoWindowErrorString);
        }

        return new DefaultCurrentWindowService(window);
    }

    public IDialogService GetDialogService<TViewModel>(TViewModel viewModel)
    {
        var view = locator.GetView(typeof(TViewModel));
        var window = windowProvider.Create(view, viewModel);
        window.Closed += Window_Closed;

        windowMap[viewModel] = window;
        return new DefaultDialogService(window);
    }

    private void Window_Closed(object sender, EventArgs e)
    {
        if (sender is Window window)
        {
            window.Closed -= Window_Closed;
        }

        windowMap.Remove(GetViewModel());

        object GetViewModel()
        {
            foreach (var pair in windowMap)
            {
                if (pair.Value == sender)
                {
                    return pair.Key;
                }
            }

            //关闭窗口时，找不到注册信息？
            throw new NotImplementedException();
        }
    }

    public IWindowService GetWindowService<TViewModel>(TViewModel viewModel)
    {
        return GetDialogService(viewModel);
    }
}
