// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using CommunityToolkit.Mvvm.Input;

namespace NatashaPad.ViewModels.Base;

internal abstract class DialogViewModelBase : ViewModelBase
{
    protected DialogViewModelBase(CommonParam commonParam) : base(commonParam)
    {
        OkCommand = new AsyncRelayCommand(OkAsync, CanOk);
        CancelCommand = new AsyncRelayCommand(CancelAsync, CanCancel);
    }

    /// <summary>
    /// 对话框的结果
    /// </summary>
    public bool Succeed { get; protected set; }

    public IAsyncRelayCommand OkCommand { get; }

    protected virtual Task OkAsync()
    {
        Succeed = true;
        CloseMe();
        return Task.CompletedTask;
    }

    protected virtual bool CanOk() => true;

    public IAsyncRelayCommand CancelCommand { get; }

    protected virtual Task CancelAsync()
    {
        CloseMe();
        return Task.CompletedTask;
    }

    protected virtual bool CanCancel() => true;
}
