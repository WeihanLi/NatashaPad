// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NatashaPad.Mvvm.MessageBox;
using NatashaPad.Mvvm.Windows;
using Prism.Mvvm;
using Avalonia.Threading;
using System.Threading.Tasks;

namespace NatashaPad.ViewModels.Base;

public abstract class ViewModelBase : BindableBase
{
    protected readonly CommonParam commonParam;

    protected ViewModelBase(CommonParam commonParam)
    {
        this.commonParam = commonParam;
    }

    protected IMediator Mediator => commonParam.Mediatr;
    protected Dispatcher Dispatcher => commonParam.Dispatcher;
    protected IServiceProvider ServiceProvider => commonParam.ServiceProvider;

    public T GetService<T>() where T : notnull
    {
        return ServiceProvider.GetRequiredService<T>();
    }

    protected void ShowMessage(string message)
    {
        GetService<IErrorMessageBoxService>().Show(message);
    }

    protected IWindowManager WindowManager => commonParam.WindowManager;

    protected Task<T> ShowDialogAsync<T>() where T : ViewModelBase
        => ShowDialogAsync(GetService<T>());

    protected async Task<T> ShowDialogAsync<T>(T vm) where T : ViewModelBase
    {
        var dialog = WindowManager.GetDialogService(vm);
        await dialog.ShowDialogAsync();
        return vm;
    }

    protected ICurrentWindowService GetCurrentView => WindowManager.GetCurrent(this);

    protected void CloseMe() => GetCurrentView.Close();
}
