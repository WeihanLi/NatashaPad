// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Prism.Commands;
using Prism.Mvvm;
using System.Windows.Input;

namespace NatashaPad.Mvvm;

public class CollectionItem : BindableBase
{
    public event EventHandler? NeedDeleteMe;

    private ICommand? _deleteThisCommand;
    public ICommand DeleteThisCommand => _deleteThisCommand ??= new DelegateCommand(FireDeleteMe);

    private void FireDeleteMe() => NeedDeleteMe?.Invoke(this, EventArgs.Empty);
}
