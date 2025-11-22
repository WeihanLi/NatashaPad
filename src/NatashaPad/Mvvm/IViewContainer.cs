// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

namespace NatashaPad.Mvvm;

public interface IViewTypeInfoLocator
{
    Type GetView(Type viewModelType);

    ViewInfo GetViewInfo(Type viewType);
}

public interface IViewInstanceLocator
{
    object GetView(Type viewModelType);
}

public static class ViewContainerExtensions
{
    public static object GetView<TViewModel>(this IViewInstanceLocator locator)
    {
        return locator.GetView(typeof(TViewModel));
    }
}
