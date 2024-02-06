﻿// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using Microsoft.Extensions.Logging.Abstractions;
using NatashaPad.Mvvm;
using NatashaPad.ViewModels.Base;
using NuGet.Versioning;
using Prism.Commands;
using ReferenceResolver;
using System.Collections.ObjectModel;
using System.Windows.Input;
using WeihanLi.Extensions;

namespace NatashaPad.ViewModels;

//TODO: 界面加载后即激活搜索框
internal partial class NugetManageViewModel : DialogViewModelBase
{
    // TODO: get it via dependency injection
    private readonly INuGetHelper _nugetHelper = new NuGetHelper(NullLoggerFactory.Instance);
    
    public NugetManageViewModel(CommonParam commonParam,
        IEnumerable<InstalledPackage> installedPackages) : base(commonParam)
    {
        InstalledPackages = new RemovableCollection<InstalledPackage>();
        installedPackages.ForEach(x => InstalledPackages.Add(x));

        SearchedPackages = new ObservableCollection<SearchedPackage>();

        Sources = _nugetHelper.GetSources().Select(x => x.Name.GetValueOrDefault(x.Source)).ToArray();
        SearchCommand = new DelegateCommand(async () => await SearchAsync());
    }

    protected override async Task OkAsync()
    {
        if (InstalledPackages.Count > 0)
        {
            await InstalledPackages.Select(p => _nugetHelper.DownloadPackage(p.Name, NuGetVersion.Parse(p.Version)))
                    .WhenAll()
                    .ConfigureAwait(false)
                ;
        }
        await base.OkAsync();
    }

    public ObservableCollection<InstalledPackage> InstalledPackages { get; }
    public ObservableCollection<SearchedPackage> SearchedPackages { get; }

    private string _searchText;
    private bool _includePrerelease = true;
    private string[] _selectedSources = [];

    public string[] Sources { get; }
    
    public string[] SelectedSources
    {
        get => _selectedSources;
        set => SetProperty(ref _selectedSources, value);
    }

    public bool IncludePrerelease
    {
        get => _includePrerelease;
        set => SetProperty(ref _includePrerelease, value);
    }

    public string SearchText
    {
        get => _searchText;
        set => SetProperty(ref _searchText, value);
    }

    public ICommand SearchCommand { get; }

    private async Task SearchAsync()
    {
        var text = _searchText?.Trim();
        if (string.IsNullOrEmpty(text))
            return;

        //TODO: 这边都给了默认值。需要在界面上支持用户选择
        var packagesNames = (
                await _nugetHelper.SearchPackages(text, _includePrerelease, sources: _selectedSources).ToArrayAsync()
                )
            .SelectMany(x => x.SearchResult.Select(r=> r.Identity.Id))
            .Distinct()
            ;

        SearchedPackages.Clear();
        foreach (var name in packagesNames)
        {
            var versions = await _nugetHelper.GetPackageVersions(
                    name, _includePrerelease, null, _selectedSources
                    ).ToArrayAsync();
            // TODO: we may want to show the source where the version comes from
            var pkg = new SearchedPackage(name,
                versions.Select(x => x.Version.ToString()).ToArray());
            pkg.InstallCommand = new DelegateCommand(
                () => InstallPackage(pkg),
                () => CanInstallPackage(pkg));

            SearchedPackages.Add(pkg);

            void InstallPackage(SearchedPackage package)
            {
                var old = InstalledPackages.FirstOrDefault(x => x.Name == package.Name);
                if (old != default)
                {
                    old.Version = package.SelectedVersion;
                }
                else
                {
                    InstalledPackages.Insert(0, new InstalledPackage(package));
                }
            }

            bool CanInstallPackage(SearchedPackage package)
            {
                return package.SelectedVersion != default;
            }
        }
    }
}
