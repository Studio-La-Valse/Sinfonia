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
}
