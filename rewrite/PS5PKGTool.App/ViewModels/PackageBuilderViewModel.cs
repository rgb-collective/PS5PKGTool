using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PS5PKGTool.Core.Services;
using System;
using System.IO;
using System.Threading.Tasks;

namespace PS5PKGTool.App.ViewModels;

public sealed partial class PackageBuilderViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _sourcePath = string.Empty;

    [ObservableProperty]
    private string _outputPath = string.Empty;

    [ObservableProperty]
    private string _contentId = string.Empty;

    [ObservableProperty]
    private string _passcode = string.Empty;

    [ObservableProperty]
    private string _status = "Ready";

    [ObservableProperty]
    private bool _isBusy;

    public PackageBuilderViewModel(MainViewModel main)
    {
        _main = main;
    }

    [RelayCommand]
    private async Task BuildAsync()
    {
        _main.ClearError();

        if (string.IsNullOrWhiteSpace(SourcePath))
        {
            Status = "Please select a source folder.";
            return;
        }

        IsBusy = true;
        Status = "Building package...";

        try
        {
            // Placeholder - actual implementation would call into Core backends
            await Task.Delay(1000);
            Status = "Package built.";
        }
        catch (Exception ex)
        {
            Status = "Error building package.";
            _main.ShowError(ex.ToString());
        }
        finally
        {
            IsBusy = false;
        }
    }
}
