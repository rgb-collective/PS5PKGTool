using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PS5PKGTool.Core.Backends;
using PS5PKGTool.Core.Models;
using PS5PKGTool.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PS5PKGTool.App.ViewModels;

public enum AppView
{
    Library,
    ImageTools,
    PackageBuilder,
    Settings,
}

public sealed partial class MainViewModel : ViewModelBase
{
    [ObservableProperty]
    private AppView _currentView = AppView.Library;

    [ObservableProperty]
    private string _title = "PS5 PKG Tool";

    [ObservableProperty]
    private string? _errorMessage;

    public LibraryViewModel Library { get; }
    public ImageToolsViewModel ImageTools { get; }
    public PackageBuilderViewModel PackageBuilder { get; }
    public SettingsViewModel Settings { get; }

    public MainViewModel()
    {
        Library = new LibraryViewModel(this);
        ImageTools = new ImageToolsViewModel(this);
        PackageBuilder = new PackageBuilderViewModel(this);
        Settings = new SettingsViewModel(this);
    }

    [RelayCommand]
    private void Navigate(AppView view) => CurrentView = view;

    [RelayCommand]
    private void CopyError()
    {
        if (!string.IsNullOrWhiteSpace(ErrorMessage))
        {
            // Write the error to the console so the user can see/copy it from the terminal
            Console.WriteLine($"=== PS5 PKG Tool Error ===");
            Console.WriteLine(ErrorMessage);
            Console.WriteLine($"=========================");
            ErrorMessage = null;
        }
    }

    /// <summary>Displays an error message in the UI error bar.</summary>
    public void ShowError(string message) => ErrorMessage = message;

    /// <summary>Clears the displayed error message.</summary>
    public void ClearError() => ErrorMessage = null;
}
