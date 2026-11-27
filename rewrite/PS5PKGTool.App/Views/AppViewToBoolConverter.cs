using System;
using System.Globalization;
using Avalonia.Data.Converters;
using PS5PKGTool.App.ViewModels;

namespace PS5PKGTool.App.Views;

/// <summary>Converts an AppView enum value to a boolean for IsVisible binding.</summary>
public sealed class AppViewToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is AppView view && parameter is string param &&
            Enum.TryParse<AppView>(param, out var target))
        {
            return view == target;
        }
        return false;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
