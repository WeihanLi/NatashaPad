// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Linq;
using NatashaPad.Mvvm;

namespace NatashaPad.ViewModels;

internal partial class NugetManageViewModel
{
    internal interface IPackage
    {
        string Name { get; }
    }

    internal class SearchedPackage : ObservableObject, IPackage
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
            set
            {
                if (SetProperty(ref _selectedVersion, value))
                {
                    InstallCommand?.NotifyCanExecuteChanged();
                }
            }
        }

        public IRelayCommand? InstallCommand { get; internal set; }
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

        public IRelayCommand UninstallCommand => DeleteThisCommand;
    }
}
