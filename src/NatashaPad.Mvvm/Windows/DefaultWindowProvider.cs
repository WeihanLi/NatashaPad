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
                    window.Title = viewInfo.Title;
                }
            }
        }

        window.DataContext = viewModel;
        return window;

        /*
         * TODO: 可选的解法：
         * 在资源文件中定义View和ViewModel的映射关系
         * 预先设计好默认window
         * window中的内容元素的内容绑定到DataContext
         * 注册时，提供name，窗口尺寸等其它信息
         * 这样，就不需要在这个方法里硬编码，把扩展性开放出去
         */
    }
}
