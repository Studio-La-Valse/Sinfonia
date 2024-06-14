using Avalonia;
using StudioLaValse.Drawable.BitmapPainters;
using Avalonia.Media;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.DrawableElements;

namespace Sinfonia.Controls;

public class _GraphicsPainter : BaseCachingBitmapPainter<DrawingContext>
{
    private readonly InteractiveControl drawingContext;

    /// <inheritdoc/>
    protected override List<Action<DrawingContext>> Cache => drawingContext.DrawActions;

    /// <inheritdoc/>
    public _GraphicsPainter(InteractiveControl drawingContext)
    {
        this.drawingContext = drawingContext;
    }

    /// <inheritdoc/>
    public override void DrawBackground(ColorARGB colorARGB)
    {
        
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableLine line)
    {
        var pen = line.Color.ToPen(line.Thickness);
        drawingContext.DrawLine(pen, line.TopLeft.ToPoint(), line.BottomRight.ToPoint());
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableRectangle rectangle)
    {
        var rect = rectangle.ToRect();
        drawingContext.FillRectangle(rectangle.Color.ToBrush(), rect);

        if (rectangle.StrokeColor != null && rectangle.StrokeWeight > 0)
        {
            var pen = rectangle.StrokeColor.ToPen(rectangle.StrokeWeight);
            drawingContext.DrawRectangle(pen, rect);
        }
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableText text)
    {
        drawingContext.DrawText(text.ToFormattedText(), new Point(text.TopLeftX, text.TopLeftY));
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawableEllipse ellipse)
    {
        var brush = ellipse.Color.ToBrush();
        var x = ellipse.CenterX - ellipse.Width / 2;
        var y = ellipse.CenterY - ellipse.Height / 2;
        var width = ellipse.Width;
        var height = ellipse.Height;
        var rect = new Rect(x, y, width, height);
        if (ellipse.StrokeColor != null && ellipse.StrokeWeight > 0)
        {
            var strokeBrush = ellipse.StrokeColor.ToBrush();
            var pen = new Pen(strokeBrush, (float)ellipse.StrokeWeight);
            drawingContext.DrawEllipse(brush, pen, rect);
        }
        else
        {
            drawingContext.DrawEllipse(brush, null, rect);
        }
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawablePolyline polyline)
    {
        var strokeBrush = polyline.Color?.ToBrush() ?? new SolidColorBrush();
        var pen = new Pen(strokeBrush, polyline.StrokeWeight);
        var points = polyline.Points.Select(p => p.ToPoint()).ToArray();
        var geometry = new PolylineGeometry(points, false);
        drawingContext.DrawGeometry(null, pen, geometry);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext drawingContext, DrawablePolygon polygon)
    {
        var fillBrush = polygon.Fill?.ToBrush() ?? new SolidColorBrush();
        var strokeBrush = polygon.Color?.ToBrush() ?? new SolidColorBrush();
        var pen = new Pen(strokeBrush, polygon.StrokeWeight);
        var points = polygon.Points.Select(p => p.ToPoint()).ToArray();
        var geometry = new PolylineGeometry(points, true);
        drawingContext.DrawGeometry(fillBrush, pen, geometry);
    }

    /// <inheritdoc/>
    protected override void DrawElement(DrawingContext canvas, DrawableBezierCurve bezier)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public override void FinishDrawing()
    {
        drawingContext.InvalidateVisual();
    }
}
