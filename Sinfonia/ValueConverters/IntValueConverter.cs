using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.ValueConverters;
public class IntValueConverter : IValueConverter
{
    public static readonly IntValueConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        return value?.ToString() ?? "0";
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is null)
        {
            return 0;
        }

        var stringValue = value.ToString();
        if (int.TryParse(stringValue, out var intValue))
        {
            return intValue;
        }

        return 0;
    }
}