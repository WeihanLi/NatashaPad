// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NatashaPad.Mvvm.MessageBox;
using NatashaPad.Mvvm.Windows;

namespace NatashaPad.Mvvm;

public static class ViewLocatorExtensions
{
    public static void UsingViewLocator(this IServiceCollection services, Action<ViewContainerOptions>? action)
    {
        var options = new ViewContainerOptions();
        action?.Invoke(options);

        foreach (var item in options)
        {
            services.AddTransient(item.ViewType);
            services.AddTransient(item.ViewModelType);
        }

        services.TryAddSingleton(new DefaultViewContainer(options));
        services.TryAddSingleton<IViewTypeInfoLocator>(s => s.GetRequiredService<DefaultViewContainer>());

        services.TryAddSingleton<IWindowManager, DefaultWindowManager>();

        services.TryAddSingleton<IViewInstanceLocator, DefaultViewLocator>();

        services.TryAddSingleton<IWindowProvider, DefaultWindowProvider>();

        services.TryAddTransient<IErrorMessageBoxService, DefaultErrorMessageBoxService>();
    }
}
