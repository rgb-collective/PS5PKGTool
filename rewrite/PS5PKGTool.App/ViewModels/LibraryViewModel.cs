using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using PS5PKGTool.Core.Models;
using PS5PKGTool.Core.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace PS5PKGTool.App.ViewModels;

public sealed partial class GameItem : ViewModelBase
{
    public string Title { get; set; } = string.Empty;
    public string TitleId { get; set; } = string.Empty;
    public string ContentId { get; set; } = string.Empty;
    public string SourceDescription { get; set; } = string.Empty;
    public string Path { get; set; } = string.Empty;
    public long Size { get; set; }
    public string Platform { get; set; } = "PS5";
}

public sealed partial class LibraryViewModel : ViewModelBase
{
    private readonly MainViewModel _main;

    [ObservableProperty]
    private ObservableCollection<GameItem> _games = [];

    [ObservableProperty]
    private GameItem? _selectedGame;

    [ObservableProperty]
    private string _status = "Ready";

    [ObservableProperty]
    private string _libraryPath = string.Empty;

    public LibraryViewModel(MainViewModel main)
    {
        _main = main;
    }

    [RelayCommand]
    public async Task ScanAsync()
    {
        _main.ClearError();

        if (string.IsNullOrWhiteSpace(LibraryPath))
        {
            Status = "Please enter a library folder path.";
            return;
        }

        Status = "Scanning...";
        Games.Clear();

        try
        {
            var scanner = new Ps5LibraryScanner();
            var result = await scanner.ScanAsync(
                new[] { LibraryPath },
                recursive: true,
                progress: new Progress<Ps5ScanProgress>(p => { /* progress updates */ }));

            foreach (var game in result.Games)
            {
                Games.Add(new GameItem
                {
                    Title = game.Title,
                    TitleId = game.TitleId,
                    ContentId = game.ContentId,
                    SourceDescription = game.SourceDescription,
                    Path = game.RootPath,
                    Size = game.SourceSize,
                    Platform = game.Platform,
                });
            }

            Status = $"Found {Games.Count} games.";
        }
        catch (Exception ex)
        {
            Status = "Error scanning library.";
            _main.ShowError(ex.ToString());
        }
    }
}
