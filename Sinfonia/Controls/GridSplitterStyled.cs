using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;

namespace Sinfonia.Controls;


public class GridSplitterStyled : GridSplitter
{
    protected override Type StyleKeyOverride { get { return typeof(GridSplitter); } }



    public GridSplitterStyled() : base()
    {

    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        Background = null;
    }


    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (Avalonia.Application.Current!.TryGetResource("SystemAccentColor", out var color))
        {
            Background = new SolidColorBrush((Color)color!);
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        Background = null;
    }
}
