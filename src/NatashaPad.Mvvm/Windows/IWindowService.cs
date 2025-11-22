// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;
using System.Threading.Tasks;

namespace NatashaPad.Mvvm.Windows;

public interface IWindowManager
{
    ICurrentWindowService GetCurrent<TViewModel>(TViewModel viewModel);

    IWindowService GetWindowService<TViewModel>(TViewModel viewModel);

    IDialogService GetDialogService<TViewModel>(TViewModel viewModel);
}

public interface IWindowService
{
    Window Window { get; }

    void Show();

    void Hide();

    void Close();

    Task ShowDialogAsync(Window? owner = null);
}

public interface IDialogService : IWindowService
{
}

public interface ICurrentWindowService : IWindowService
{
}
