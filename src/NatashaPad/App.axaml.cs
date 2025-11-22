// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NatashaPad.Mvvm;
using NatashaPad.Mvvm.Windows;
using NatashaPad.ViewModels;
using NatashaPad.Views;
using ReferenceResolver;
using WeihanLi.Common;

namespace NatashaPad;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        Init();
        base.OnFrameworkInitializationCompleted();
    }

    private void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<IReferenceResolver, FileReferenceResolver>();
        services.AddSingleton<IReferenceResolver, NuGetReferenceResolver>();
        services.AddSingleton<INScriptEngine, CSharpScriptEngine>();
        services.AddTransient<NScriptOptions>();
        services.TryAddSingleton<DumperResolver>();
        services.AddSingleton<IDumper, DefaultDumper>();

        services.AddTransient<CommonParam>();

        services.AddMediatR(typeof(App));

        services.AddSingleton(_ => Dispatcher.UIThread);
        services.AddReferenceResolvers();

        services.UsingViewLocator(options =>
        {
            options.Register<MainWindow, MainViewModel>();
            options.Register<UsingManageView, UsingManageViewModel>(opt =>
            {
                opt.Width = 600;
                opt.Height = 400;
                opt.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterScreen;
                opt.Title = Properties.Resource.UsingManageTitleString;
            });
            options.Register<NugetManageView, NugetManageViewModel>(opt =>
            {
                opt.Width = 800;
                opt.Height = 450;
                opt.WindowStartupLocation = Avalonia.Controls.WindowStartupLocation.CenterScreen;
                opt.Title = Properties.Resource.NugetManageTitleString;
            });
        });
    }

    private void Init()
    {
        IServiceCollection services = new ServiceCollection();
        ConfigureServices(services);
        DependencyResolver.SetDependencyResolver(services);

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var windowManager = DependencyResolver.ResolveRequiredService<IWindowManager>();
            var mainViewModel = DependencyResolver.ResolveRequiredService<MainViewModel>();
            var windowService = windowManager.GetWindowService(mainViewModel);

            desktop.MainWindow = windowService.Window;
            windowService.Show();
        }
    }
}
