// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;
using System.Collections.ObjectModel;

namespace NatashaPad.Mvvm;

public class ViewContainerOptions : Collection<RegisterInfo>
{
    public void Register<TView, TViewModel>()
    {
        Add(new RegisterInfo(typeof(TView), typeof(TViewModel)));
    }

    public void Register<TView, TViewModel>(Action<ViewInfo>? action)
    {
        var reg = new RegisterInfo(typeof(TView), typeof(TViewModel));

        var viewInfo = new ViewInfo();
        action?.Invoke(viewInfo);
        reg.ViewInfo = viewInfo;

        Add(reg);
    }
}

public sealed class RegisterInfo
{
    public RegisterInfo(Type viewType, Type viewModelType, string name)
    {
        ViewType = viewType;
        ViewModelType = viewModelType;
        Name = name;
    }

    public RegisterInfo(Type viewType, Type viewModelType) : this(viewType, viewModelType, string.Empty)
    {
    }

    public string Name { get; }

    public Type ViewType { get; }
    public Type ViewModelType { get; }

    public ViewInfo ViewInfo { get; set; }
}

public class ViewInfo
{
    public double? Width { get; set; }
    public double? Height { get; set; }

    public string? Title { get; set; }

    public WindowStartupLocation WindowStartupLocation { get; set; } = WindowStartupLocation.Manual;

    public SizeToContent? SizeToContent { get; set; }
}
