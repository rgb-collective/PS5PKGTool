using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PS5PKGTool.Core.Builders;
using PS5PKGTool.Core.Services;
using PS5PKGTool.Ffpfsc;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
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
    private CancellationTokenSource? _cts;

    [ObservableProperty]
    private string _sourcePath = string.Empty;

    [ObservableProperty]
    private string _outputPath = string.Empty;

    [ObservableProperty]
    private ImageToolAction _selectedAction = ImageToolAction.Extract;

    [ObservableProperty]
    private string _status = "Ready";

    [ObservableProperty]
    private bool _isBusy;

    [ObservableProperty]
    private bool _overwrite;

    [ObservableProperty]
    private string _passcode = SonyDebugPackageCredentials.DefaultPasscode;

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
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var ct = _cts.Token;

        if (string.IsNullOrWhiteSpace(SourcePath))
        {
            Status = "Please select a source.";
            return;
        }

        if (string.IsNullOrWhiteSpace(OutputPath))
        {
            Status = "Please select an output path.";
            return;
        }

        IsBusy = true;
        Status = "Working...";

        try
        {
            switch (SelectedAction)
            {
                case ImageToolAction.Extract:
                    await ExtractAsync(ct);
                    break;
                case ImageToolAction.Convert:
                    await ConvertAsync(ct);
                    break;
                case ImageToolAction.Verify:
                    await VerifyAsync(ct);
                    break;
                case ImageToolAction.Repair:
                    Status = "Repair is not yet implemented.";
                    break;
                default:
                    throw new InvalidOperationException($"Unknown action: {SelectedAction}");
            }
        }
        catch (OperationCanceledException)
        {
            Status = "Cancelled.";
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

    [RelayCommand]
    private void Cancel()
    {
        _cts?.Cancel();
    }

    private async Task ExtractAsync(CancellationToken ct)
    {
        var result = await SonyPackageExtraction.ExtractAsync(
            SourcePath, OutputPath, Passcode,
            progress: new Progress<SonyPackageExtractProgress>(p =>
            {
                Status = $"Extracting {Path.GetFileName(p.CurrentPath)} ({p.CompletedBytes / 1024 / 1024} MB / {p.TotalBytes / 1024 / 1024} MB)";
            }),
            cancellationToken: ct);

        Status = $"Extracted {result.FileCount} files ({result.ExtractedBytes / 1024 / 1024} MB) to {result.Destination}";
    }

    private async Task ConvertAsync(CancellationToken ct)
    {
        var target = Ps5ImageConversionTarget.Exfat;
        var result = await SonyPackageImageConversion.ConvertAsync(
            SourcePath, OutputPath, target, Overwrite,
            progress: new Progress<Ps5ImageConversionProgress>(p =>
            {
                Status = $"{p.Stage} ({p.Completed} / {p.Total})";
            }),
            cancellationToken: ct);

        Status = $"Converted to {result.Target} at {OutputPath}";
    }

    private async Task VerifyAsync(CancellationToken ct)
    {
        var result = PackageReaderVerification.Inspect(SourcePath, Passcode);
        Status = $"Verified: {result.FileCount} files{(result.HasEboot ? ", eboot.bin found" : "")}";
    }
}
