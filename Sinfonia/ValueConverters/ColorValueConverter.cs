using Syncfusion.Windows.Shared;
using System.Globalization;
using System.Windows.Data;
using Color = System.Windows.Media.Color;

namespace Sinfonia.ValueConverters
{
    [ValueConversion(typeof(ColorARGB), typeof(Color))]
    public class ColorValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (ColorARGB)value;
            var target = Color.FromArgb((byte)source.Alpha, (byte)source.Red, (byte)source.Green, (byte)source.Blue);
            return target;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (Color)value;
            var target = new ColorARGB(source.A, source.R, source.G, source.B);
            return target;
        }
    }

    public class ColorHexToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var source = (ColorARGB)value;
            var target = Color.FromArgb((byte)source.Alpha, (byte)source.Red, (byte)source.Green, (byte)source.Blue);
            var _Color = ColorEdit.SuchColor(target)[0];
            return _Color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var hex = (Color)value;
            var target = new ColorARGB(hex.A, hex.R, hex.G, hex.B);
            return target;
        }
    }
}
