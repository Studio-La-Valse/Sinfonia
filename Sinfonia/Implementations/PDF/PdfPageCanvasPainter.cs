using PdfSharp.Drawing;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.DrawableElements;

namespace Sinfonia.Implementations.PDF;

internal class PdfPageCanvasPainter : BaseLazyBitmapPainter<XGraphics>
{
    private readonly XGraphics graphics;
    private readonly int pageWidth;
    private readonly int pageHeight;

    public PdfPageCanvasPainter(XGraphics page, int pageWidth, int pageHeight) : base(page)
    {
        this.graphics = page;
        this.pageWidth = pageWidth;
        this.pageHeight = pageHeight;
    }

    public override void DrawBackground(ColorARGB color)
    {
        var rect = new XRect()
        {
            Height = pageHeight,
            Width = pageWidth,
        };
        var brush = new XSolidBrush()
        {
            Color = color.ToXColor()
        };
        graphics.DrawRectangle(brush, rect);
    }

    public override void InitDrawing()
    {

    }

    protected override void DrawElement(XGraphics bitmap, DrawableLine line)
    {
        var pen = line.Color.ToXPen(line.Thickness);
        var firstPoint = new XY(line.X1, line.Y1).ToXPoint();
        var secondPoint = new XY(line.X2, line.Y2).ToXPoint();

        graphics.DrawLine(pen, firstPoint, secondPoint);
    }

    protected override void DrawElement(XGraphics bitmap, DrawableRectangle rectangle)
    {
        var brush = new XSolidBrush()
        {
            Color = rectangle.Color.ToXColor()
        };

        var rect = new XRect()
        {
            Height = rectangle.Height,
            Width = rectangle.Width,
            X = rectangle.TopLeftX,
            Y = rectangle.TopLeftY,
        };

        if (rectangle.StrokeColor is not null && rectangle.StrokeWeight > 0)
        {
            var color = rectangle.StrokeColor.ToXColor();
            var pen = new XPen(color, rectangle.StrokeWeight);
            graphics.DrawRectangle(pen, brush, rect);
        }
        else
        {
            graphics.DrawRectangle(brush, rect);
        }
    }


    protected override void DrawElement(XGraphics bitmap, DrawableText text)
    {
        var xfont = text.ToXFont();
        var brush = new XSolidBrush()
        {
            Color = text.Color.ToXColor()
        };
        var point = new XY(text.TopLeftX, text.GetBoundingBox().Center.Y).ToXPoint();
        graphics.DrawString(text.Text, xfont, brush, point);
    }

    protected override void DrawElement(XGraphics bitmap, DrawableEllipse ellipse)
    {
        var brush = new XSolidBrush()
        {
            Color = ellipse.Color.ToXColor()
        };
        var rect = new XRect()
        {
            Height = ellipse.Height,
            Width = ellipse.Width,
            X = ellipse.CenterX - ellipse.Width / 2,
            Y = ellipse.CenterY - ellipse.Height / 2,
        };
        if (ellipse.StrokeColor is not null && ellipse.StrokeWeight > 0)
        {
            var color = ellipse.StrokeColor.ToXColor();
            var pen = new XPen(color, ellipse.StrokeWeight);
            graphics.DrawEllipse(pen, brush, rect);
        }
        else
        {
            graphics.DrawEllipse(brush, rect);
        }
    }

    protected override void DrawElement(XGraphics bitmap, DrawablePolyline polyline)
    {
        var pen = polyline.Color.ToXPen(polyline.StrokeWeight);
        var points = polyline.Points.Select(p => p.ToXPoint()).ToArray();
        graphics.DrawLines(pen, points);
    }

    protected override void DrawElement(XGraphics bitmap, DrawablePolygon polygon)
    {
        if (polygon.Color is null && polygon.Fill is null)
        {
            return;
        }

        var points = polygon.Points.Select(p => p.ToXPoint()).ToArray();

        if (polygon.Color is not null && polygon.StrokeWeight > 0)
        {
            var color = polygon.Color.ToXColor();
            var pen = new XPen(color, polygon.StrokeWeight);

            if (polygon.Fill is not null)
            {
                var brush = new XSolidBrush()
                {
                    Color = polygon.Fill.ToXColor()
                };
                graphics.DrawPolygon(pen, brush, points, XFillMode.Alternate);
                return;
            }
            graphics.DrawPolygon(pen, points);
            return;
        }

        if (polygon.Fill is not null)
        {
            var color = polygon.Fill.ToXColor();
            var brush = new XSolidBrush()
            {
                Color = polygon.Fill.ToXColor()
            };
            graphics.DrawPolygon(brush, points, XFillMode.Alternate);
        }
    }

    protected override void DrawElement(XGraphics bitmap, DrawableBezierCurve bezier)
    {
        var path = new XGraphicsPath();
        var points = bezier.Points.Select(p => p.ToXPoint()).ToArray();
        var color = bezier.Color.ToXColor();
        var pen = new XPen(color, bezier.StrokeWeight);
        graphics.DrawBeziers(pen, points);
    }
}

public static class XELementExtensions
{
    public static XColor ToXColor(this ColorARGB colorARGB)
    {
        var color = new XColor()
        {
            A = colorARGB.Alpha / 255d,
            R = (byte)colorARGB.Red,
            G = (byte)colorARGB.Green,
            B = (byte)colorARGB.Blue
        };
        return color;
    }
    public static XPen ToXPen(this ColorARGB colorARGB, double thickness)
    {
        var color = colorARGB.ToXColor();
        var pen = new XPen(color, thickness);
        return pen;
    }
    public static XPoint ToXPoint(this XY xY)
    {
        var firstPoint = new XPoint()
        {
            X = xY.X,
            Y = xY.Y,
        };
        return firstPoint;
    }
    public static XFont ToXFont(this DrawableText drawableText)
    {
        var fontFamilyCore = drawableText.FontFamily;
        var fontPath = fontFamilyCore.Uri is null ?
            fontFamilyCore.Name :
            System.IO.Path.Combine(fontFamilyCore.Uri.ToString(), fontFamilyCore.Name);
        var xfont = new XFont(fontPath, drawableText.FontSize);
        return xfont;
    }
}
