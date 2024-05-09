using Avalonia;
using Avalonia.Controls;
using StudioLaValse.Drawable.Avalonia.Painters;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.Interaction.UserInput;
using StudioLaValse.Drawable.Interaction.Extensions;
using StudioLaValse.Geometry;
using StudioLaValse.Key;
using System;
using Avalonia.ReactiveUI;
using Avalonia.Media;
using StudioLaValse.Drawable.Avalonia.Controls;
using StudioLaValse.Drawable.DrawableElements;
using StudioLaValse.Drawable.Text;

namespace Sinfonia.Controls;

internal static class GeometryExtensions
{
    public static Point ToPoint(this XY point)
    {
        return new Point(point.X, point.Y);
    }

    public static Color ToColor(this ColorARGB colorARGB)
    {
        var color = new Color((byte)colorARGB.Alpha, (byte)colorARGB.Red, (byte)colorARGB.Green, (byte)colorARGB.Blue);
        return color;
    }

    public static SolidColorBrush ToBrush(this ColorARGB colorARGB)
    {
        var color = colorARGB.ToColor();
        var brush = new SolidColorBrush(color);
        return brush;
    }

    public static Pen ToPen(this ColorARGB colorARGB, double thickness)
    {
        var brush = colorARGB.ToBrush();
        var pen = new Pen(brush, thickness);
        return pen;
    }

    public static Rect ToRect(this DrawableRectangle rectangle)
    {
        var rect = new Rect(new Point(rectangle.TopLeftX, rectangle.TopLeftY), new Size(rectangle.Width, rectangle.Height));
        return rect;
    }

    public static FormattedText ToFormattedText(this DrawableText text)
    {
        var brush = text.Color.ToBrush();
        var fontFamily = text.FontFamily.ToFontFamily();
        var formattedText = new FormattedText(text.Text, System.Globalization.CultureInfo.InvariantCulture, FlowDirection.LeftToRight, new Typeface(fontFamily), text.FontSize, brush);
        return formattedText;
    }

    public static FontFamily ToFontFamily(this FontFamilyCore fontFamily)
    {
        return fontFamily.Uri is null ? new FontFamily(fontFamily.Name) : new FontFamily(fontFamily.Uri, fontFamily.Name);
    }
}

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

public partial class CanvasControl : UserControl, IObserver<IUniqueScoreElement>
{
    public CanvasControl()
    {
        InitializeComponent();

        BaseBitmapPainter = new _GraphicsPainter(canvas);

        PanEnabledDisposable = canvas.EnablePan();
        ZoomEnabledDisposable = canvas.EnableZoom();
    }

    public static readonly DirectProperty<CanvasControl, SceneManager<IUniqueScoreElement, int>> SceneManagerProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, SceneManager<IUniqueScoreElement, int>>(nameof(SceneManager), o => o.SceneManager, (o, v) => o.SceneManager = v);

    public static readonly DirectProperty<CanvasControl, IObservable<BoundingBox>> SelectionBorderProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, IObservable<BoundingBox>>(nameof(SelectionBorder), o => o.SelectionBorder, (o, v) => o.SelectionBorder = v);

    public static readonly DirectProperty<CanvasControl, bool> EnablePanProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, bool>(nameof(EnablePan), o => o.EnablePan, (o, v) => o.EnablePan = v);

    public static readonly DirectProperty<CanvasControl, INotifyEntityChanged<IUniqueScoreElement>> InvalidatorProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, INotifyEntityChanged<IUniqueScoreElement>>(nameof(Invalidator), o => o.Invalidator, (o, v) => o.Invalidator = v);

    public static readonly DirectProperty<CanvasControl, IPipe> PipeProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, IPipe>(nameof(Pipe), o => o.Pipe, (o, v) => o.Pipe = v);


    private SceneManager<IUniqueScoreElement, int> sceneManager;
    public SceneManager<IUniqueScoreElement, int> SceneManager
    {
        get => sceneManager;
        set
        {
            SetAndRaise(SceneManagerProperty, ref sceneManager, value);
            value?.Rerender(BaseBitmapPainter);
        }
    }
    private IObservable<BoundingBox> _selectionBorder;
    public IObservable<BoundingBox> SelectionBorder
    {
        get => _selectionBorder;
        set
        {
            SetAndRaise(SelectionBorderProperty, ref _selectionBorder, value);
            SelectionBorderDisposable?.Dispose();
            SelectionBorderDisposable = value?.Subscribe(selectionBorder.CreateObserver(InteractiveCanvas));
        }
    }

    private bool enablePan;
    public bool EnablePan
    {
        get => enablePan;
        set
        {
            SetAndRaise(EnablePanProperty, ref enablePan, value);
            PanEnabledDisposable?.Dispose();
            ZoomEnabledDisposable?.Dispose();

            if (value)
            {
                PanEnabledDisposable = InteractiveCanvas.EnablePan();
                ZoomEnabledDisposable = InteractiveCanvas.EnableZoom();
            }
        }
    }

    private INotifyEntityChanged<IUniqueScoreElement> invalidator;
    public INotifyEntityChanged<IUniqueScoreElement> Invalidator
    {
        get => invalidator;
        set
        {
            SetAndRaise(InvalidatorProperty, ref invalidator, value);
            InvalidatorDisposable?.Dispose();
            InvalidatorDisposable = value?.Subscribe(this);
        }
    }
    private IPipe pipe;
    public IPipe Pipe
    {
        get => pipe;
        set
        {
            SetAndRaise(PipeProperty, ref pipe, value);
            PipeDisposable?.Dispose();
            if (value is not null)
            {
                PipeDisposable = InteractiveCanvas.Subscribe(value);
            }
        }
    }



    public IDisposable? PanEnabledDisposable { get; set; }
    public IDisposable? ZoomEnabledDisposable { get; set; }
    public IDisposable? InvalidatorDisposable { get; set; }
    public IDisposable? SelectionBorderDisposable { get; set; }
    public IDisposable? PipeDisposable { get; set; }
    public BaseBitmapPainter BaseBitmapPainter { get; }
    public IInteractiveCanvas InteractiveCanvas => canvas;





    public void OnCompleted()
    {
        if (SceneManager is null)
        {
            throw new Exception("No scenemanager active to invalidate this element.");
        }

        SceneManager.RenderChanges(BaseBitmapPainter);
    }
    public void OnError(Exception error)
    {
        throw error;
    }
    public void OnNext(IUniqueScoreElement value)
    {
        if (SceneManager is null)
        {
            throw new Exception("No scenemanager active to invalidate this element.");
        }

        SceneManager.AddToQueue(value);
    }
}
