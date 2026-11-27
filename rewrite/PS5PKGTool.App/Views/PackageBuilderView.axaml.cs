using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using PS5PKGTool.App.ViewModels;

namespace PS5PKGTool.App.Views;

public partial class PackageBuilderView : UserControl
{
    public PackageBuilderView()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
