using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PS5PKGTool.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace PS5PKGTool.App.ViewModels;

public enum ImageToolAction
{
    Convert,
    Extract,
    Verify,
    Repair,
}

public sealed partial class ImageToolsViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private string _sourcePath = string.Empty;

    [ObservableProperty]
    private string _outputPath = string.Empty;

    [ObservableProperty]
    private ImageToolAction _selectedAction = ImageToolAction.Convert;

    [ObservableProperty]
    private string _status = "Ready";

    [ObservableProperty]
    private bool _isBusy;

    public ObservableCollection<ImageToolAction> AvailableActions { get; } =
        new() { ImageToolAction.Convert, ImageToolAction.Extract, ImageToolAction.Verify, ImageToolAction.Repair };

    public ImageToolsViewModel(MainViewModel main)
    {
        _main = main;
    }

    [RelayCommand]
    private async Task ExecuteAsync()
    {
        _main.ClearError();

        if (string.IsNullOrWhiteSpace(SourcePath))
        {
            Status = "Please select a source.";
            return;
        }

        IsBusy = true;
        Status = "Working...";

        try
        {
            // Placeholder - actual implementation would call into Core services
            await Task.Delay(1000);
            Status = $"Completed: {SelectedAction}";
        }
        catch (Exception ex)
        {
            Status = "Error executing image tool.";
            _main.ShowError(ex.ToString());
        }
        finally
        {
            IsBusy = false;
        }
    }
}
