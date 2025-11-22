// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;

namespace NatashaPad.Mvvm.Windows;

public interface IWindowProvider
{
    Window Create(object view, object viewModel);
}
