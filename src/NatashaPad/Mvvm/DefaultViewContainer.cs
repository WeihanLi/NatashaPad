// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Microsoft.Extensions.DependencyInjection;

namespace NatashaPad.Mvvm;

internal class DefaultViewContainer : IViewTypeInfoLocator
{
    private readonly Dictionary<Type, Type> vmToviewMap;
    private readonly Dictionary<Type, ViewInfo> viewToInfoMap;

    public DefaultViewContainer()
    {
        vmToviewMap = new Dictionary<Type, Type>();
        viewToInfoMap = new Dictionary<Type, ViewInfo>();
    }

    internal DefaultViewContainer(IEnumerable<RegisterInfo> infos)
    {
        vmToviewMap = infos.ToDictionary(x => x.ViewModelType, x => x.ViewType);
        viewToInfoMap = infos.ToDictionary(x => x.ViewType, x => x.ViewInfo);
    }

    public Type GetView(Type vmType)
    {
        if (!vmToviewMap.TryGetValue(vmType, out var type))
        {
            throw new KeyNotFoundException($"No view type registered for view model '{vmType.Name}'.");
        }

        return type;
    }

    public ViewInfo GetViewInfo(Type viewType)
    {
        if (!viewToInfoMap.TryGetValue(viewType, out var info))
        {
            throw new KeyNotFoundException($"No view metadata registered for view '{viewType.Name}'.");
        }

        return info;
    }
}

internal class DefaultViewLocator : IViewInstanceLocator
{
    private readonly IViewTypeInfoLocator viewLocator;
    private readonly IServiceProvider serviceProvider;

    public DefaultViewLocator(
        IViewTypeInfoLocator viewLocator,
        IServiceProvider serviceProvider)
    {
        this.viewLocator = viewLocator;
        this.serviceProvider = serviceProvider;
    }

    public object GetView(Type viewModelType)
    {
        return serviceProvider.GetRequiredService(viewLocator.GetView(viewModelType));
    }
}
