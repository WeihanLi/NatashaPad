// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using NatashaPad.Mvvm;
using Prism.Mvvm;
using System.Windows.Input;

namespace NatashaPad.ViewModels;

internal partial class NugetManageViewModel
{
    internal interface IPackage
    {
        string Name { get; }
    }

    internal class SearchedPackage : BindableBase, IPackage
    {
        public string Name { get; }

        public SearchedPackage(string name,
            IEnumerable<string> versions)
        {
            Name = name;
            Versions = versions.Reverse().ToArray();
            _selectedVersion = Versions.FirstOrDefault() ?? string.Empty;
        }

        public IEnumerable<string> Versions { get; }

        private string _selectedVersion = string.Empty;

        public string SelectedVersion
        {
            get => _selectedVersion;
            set => SetProperty(ref _selectedVersion, value);
        }

        public ICommand InstallCommand { get; internal set; } = default!;
    }

    internal class InstalledPackage : CollectionItem, IPackage
    {
        public string Name { get; }

        public InstalledPackage(string name,
            string version)
        {
            Name = name;
            Version = version;
        }

        public InstalledPackage(SearchedPackage searchedPackage)
            : this(searchedPackage.Name, searchedPackage.SelectedVersion)
        { }

        private string _version = string.Empty;

        public string Version
        {
            get => _version;
            internal set => SetProperty(ref _version, value);
        }

        public ICommand UninstallCommand => DeleteThisCommand;
    }
}
