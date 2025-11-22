// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using CommunityToolkit.Mvvm.Input;
using NatashaPad.Mvvm;
using NatashaPad.ViewModels.Base;
using System.Collections.Generic;
using System.Linq;

namespace NatashaPad.ViewModels;

//TODO: 添加时，1. 获取光标；2. 滚动到空处；3. 空处红框显示；
internal sealed class UsingManageViewModel : DialogViewModelBase
{
    public UsingManageViewModel(CommonParam commonParam,
        IEnumerable<string> namespaces) : base(commonParam)
    {
        AllItems = new RemovableCollection<NamespaceItem>();
        foreach (var item in namespaces.Distinct())
        {
            AllItems.Add(new NamespaceItem(item));
        }

        AddCommand = new RelayCommand(Add);
    }

    public IRelayCommand AddCommand { get; }

    private void Add()
    {
        if (!AllItems.Any(x => x.IsEmpty))
            AllItems.Insert(0, new NamespaceItem(""));
    }

    public RemovableCollection<NamespaceItem> AllItems { get; }

    protected override Task OkAsync()
    {
        var empties = AllItems.Where(x => x.IsEmpty);
        var duplicates = AllItems.GroupBy(x => x.Namespace)
            .SelectMany(x => x.Skip(1));

        foreach (var item in empties.Concat(duplicates).ToArray())
        {
            AllItems.Remove(item);
        }
        return base.OkAsync();
    }
}

internal sealed class NamespaceItem : CollectionItem
{
    public NamespaceItem(string @namespace)
    {
        _namespace = @namespace;
    }

    private string _namespace;

    public string Namespace
    {
        get => _namespace;
        set => SetProperty(ref _namespace, value);
    }

    internal bool IsEmpty => string.IsNullOrWhiteSpace(_namespace);

    public override bool Equals(object? obj)
    {
        return obj is NamespaceItem item &&
               _namespace == item._namespace;
    }

    public override int GetHashCode()
    {
        // ReSharper disable once NonReadonlyMemberInGetHashCode
        return _namespace.GetHashCode();
    }
}
