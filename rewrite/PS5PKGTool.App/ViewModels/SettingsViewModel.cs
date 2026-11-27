using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace PS5PKGTool.App.ViewModels;

public sealed partial class SettingsViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _theme = "Dark";

    [ObservableProperty]
    private string _defaultOutputFolder = string.Empty;

    [ObservableProperty]
    private string _status = "Settings";

    public ObservableCollection<string> AvailableThemes { get; } = new() { "Dark", "Light" };

    public SettingsViewModel(MainViewModel main)
    {
        _main = main;
    }
}
