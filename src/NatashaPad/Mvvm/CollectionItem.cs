// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace NatashaPad.Mvvm;

public class CollectionItem : ObservableObject
{
    public event EventHandler? NeedDeleteMe;

    private IRelayCommand? _deleteThisCommand;
    public IRelayCommand DeleteThisCommand => _deleteThisCommand ??= new RelayCommand(FireDeleteMe);

    private void FireDeleteMe() => NeedDeleteMe?.Invoke(this, EventArgs.Empty);
}
