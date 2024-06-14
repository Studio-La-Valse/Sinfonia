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
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Reader.Extensions;
using Avalonia.Interactivity;

namespace Sinfonia.Controls;

public partial class CanvasControl : UserControl, IObserver<InvalidationRequest<IUniqueScoreElement>>
{
    private SceneManager<IUniqueScoreElement, int>? sceneManager;
    private IObservable<BoundingBox>? _selectionBorder;
    private bool enablePan;
    private INotifyEntityChanged<IUniqueScoreElement>? invalidator;
    private IPipe? pipe;


    public static readonly DirectProperty<CanvasControl, SceneManager<IUniqueScoreElement, int>?> SceneManagerProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, SceneManager<IUniqueScoreElement, int>?>(nameof(SceneManager), o => o.SceneManager, (o, v) => o.SceneManager = v);

    public static readonly DirectProperty<CanvasControl, IObservable<BoundingBox>?> SelectionBorderProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, IObservable<BoundingBox>?>(nameof(SelectionBorder), o => o.SelectionBorder, (o, v) => o.SelectionBorder = v);

    public static readonly DirectProperty<CanvasControl, bool> EnablePanProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, bool>(nameof(EnablePan), o => o.EnablePan, (o, v) => o.EnablePan = v);

    public static readonly DirectProperty<CanvasControl, INotifyEntityChanged<IUniqueScoreElement>?> InvalidatorProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, INotifyEntityChanged<IUniqueScoreElement>?>(nameof(Invalidator), o => o.Invalidator, (o, v) => o.Invalidator = v);

    public static readonly DirectProperty<CanvasControl, IPipe?> PipeProperty = AvaloniaProperty
        .RegisterDirect<CanvasControl, IPipe?>(nameof(Pipe), o => o.Pipe, (o, v) => o.Pipe = v);

    
    
    public SceneManager<IUniqueScoreElement, int>? SceneManager
    {
        get => sceneManager;
        set
        {
            SetAndRaise(SceneManagerProperty, ref sceneManager, value);
            value?.Rerender(BaseBitmapPainter);
        }
    }
    public IObservable<BoundingBox>? SelectionBorder
    {
        get => _selectionBorder;
        set
        {
            SetAndRaise(SelectionBorderProperty, ref _selectionBorder, value);
            SelectionBorderDisposable?.Dispose();
            SelectionBorderDisposable = value?.Subscribe(selectionBorder.CreateObserver(canvas));
        }
    }
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
                PanEnabledDisposable = canvas.EnablePan();
                ZoomEnabledDisposable = canvas.EnableZoom();
            }
        }
    }
    public INotifyEntityChanged<IUniqueScoreElement>? Invalidator
    {
        get => invalidator;
        set
        {
            SetAndRaise(InvalidatorProperty, ref invalidator, value);
            InvalidatorDisposable?.Dispose();
            InvalidatorDisposable = value?.Subscribe(this);
        }
    }
    public IPipe? Pipe
    {
        get => pipe;
        set
        {
            SetAndRaise(PipeProperty, ref pipe, value);
            PipeDisposable?.Dispose();
            if (value is not null)
            {
                PipeDisposable = canvas.Subscribe(value);
            }
        }
    }



    public IDisposable? PanEnabledDisposable { get; set; }
    public IDisposable? ZoomEnabledDisposable { get; set; }
    public IDisposable? InvalidatorDisposable { get; set; }
    public IDisposable? SelectionBorderDisposable { get; set; }
    public IDisposable? PipeDisposable { get; set; }
    public BaseBitmapPainter BaseBitmapPainter { get; }


    public CanvasControl()
    {
        InitializeComponent();

        BaseBitmapPainter = new _GraphicsPainter(canvas);

        PanEnabledDisposable = canvas.EnablePan();

        ZoomEnabledDisposable = canvas.EnableZoom();
    }


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
    public void OnNext(InvalidationRequest<IUniqueScoreElement> value)
    {
        if (SceneManager is null)
        {
            throw new Exception("No scenemanager active to invalidate this element.");
        }

        SceneManager.AddToQueue(value);
    }
}
