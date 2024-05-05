using Avalonia.Controls;
using Avalonia.Media;
using StudioLaValse.Drawable.Text;

namespace Sinfonia.Controls;

public partial class MainControl : UserControl
{
    public MainControl()
    {
        InitializeComponent();
    }

    public override void Render(DrawingContext context)
    {
        ExternalTextMeasure.TextMeasurer = new AvaloniaTextMeasurer();

        base.Render(context);
    }
}

public class AvaloniaTextMeasurer : IMeasureText
{
    public XY Measure(string text, FontFamilyCore fontFamilyCore, double size)
    {
        var fontFamily = new FontFamily(fontFamilyCore.Uri, fontFamilyCore.Name);
        var formattedText = new FormattedText(text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), size, Brushes.White);
        var xy = new XY(formattedText.Width, formattedText.Height);
        return xy;
    }
}
