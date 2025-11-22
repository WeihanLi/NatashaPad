// Copyright (c) NatashaPad. All rights reserved.
// Licensed under the Apache license.

using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Logging.Abstractions;
using NatashaPad.Mvvm;
using NatashaPad.ViewModels.Base;
using NuGet.Versioning;
using ReferenceResolver;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace NatashaPad.ViewModels;

//TODO: 界面加载后即激活搜索框
internal partial class NuGetManageViewModel : DialogViewModelBase
{
    // TODO: get it via dependency injection
    private readonly INuGetHelper _nugetHelper = new NuGetHelper(NullLoggerFactory.Instance);
    
    public NuGetManageViewModel(CommonParam commonParam,
        IEnumerable<InstalledPackage> installedPackages) : base(commonParam)
    {
        InstalledPackages = new RemovableCollection<InstalledPackage>();
        foreach (var package in installedPackages)
        {
            InstalledPackages.Add(package);
        }

        SearchedPackages = new ObservableCollection<SearchedPackage>();

        Sources = _nugetHelper.GetSources()
            .Select(x => string.IsNullOrWhiteSpace(x.Name) ? x.Source : x.Name)
            .ToArray();
        SearchCommand = new AsyncRelayCommand(SearchAsync);
    }

    protected override async Task OkAsync()
    {
        if (InstalledPackages.Count > 0)
        {
            var downloads = InstalledPackages
                .Select(p => _nugetHelper.DownloadPackage(p.Name, NuGetVersion.Parse(p.Version)));
            await Task.WhenAll(downloads).ConfigureAwait(false);
        }
        await base.OkAsync();
    }

    public ObservableCollection<InstalledPackage> InstalledPackages { get; }
    public ObservableCollection<SearchedPackage> SearchedPackages { get; }

    private string _searchText = string.Empty;
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

    public IAsyncRelayCommand SearchCommand { get; }

    private async Task SearchAsync()
    {
        var text = _searchText?.Trim();
        if (string.IsNullOrEmpty(text))
            return;

        //TODO: 这边都给了默认值。需要在界面上支持用户选择
        var packageNames = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        await foreach (var result in _nugetHelper.SearchPackages(text, _includePrerelease, sources: _selectedSources))
        {
            foreach (var metadata in result.SearchResult)
            {
                packageNames.Add(metadata.Identity.Id);
            }
        }

        SearchedPackages.Clear();
        foreach (var name in packageNames)
        {
            var versionBuffer = new List<NuGetVersion>();
            await foreach (var versionInfo in _nugetHelper.GetPackageVersions(
                           name, _includePrerelease, false, null, _selectedSources))
            {
                versionBuffer.Add(versionInfo.Version);
            }
            // TODO: we may want to show the source where the version comes from
            var pkg = new SearchedPackage(name,
                versionBuffer.Select(x => x.Version.ToString()).ToArray());
            var installCommand = new RelayCommand(
                () => InstallPackage(pkg),
                () => CanInstallPackage(pkg));
            pkg.InstallCommand = installCommand;
            installCommand.NotifyCanExecuteChanged();

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
