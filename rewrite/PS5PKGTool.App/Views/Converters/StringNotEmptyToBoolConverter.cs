using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace PS5PKGTool.App.Views.Converters;

/// <summary>Converts a string to true when it is non-empty, for use with IsVisible bindings.</summary>
public sealed class StringNotEmptyToBoolConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => !string.IsNullOrWhiteSpace(value as string);

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
