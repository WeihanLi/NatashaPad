// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace NatashaPad.Views;

public partial class NugetManageView : UserControl
{
    public NugetManageView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
