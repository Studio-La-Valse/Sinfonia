using Sinfonia.Implementations;
using StudioLaValse.Drawable.BitmapPainters;
using StudioLaValse.Drawable.WPF.DependencyProperties;
using StudioLaValse.Drawable.WPF.Painters;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Sinfonia.UserControls
{
    /// <summary>
    /// Interaction logic for CanvasUserControl.xaml
    /// </summary>
    public partial class CanvasUserControl : UserControl
    {
        public static readonly DependencyProperty SceneProperty = DependencyPropertyBase
            .Register<CanvasUserControl, SceneManager<IUniqueScoreElement, int>>(nameof(SceneManager), (o, e) => { e.Rerender(o.BaseBitmapPainter); });

        public static readonly DependencyProperty SelectionBorderProperty = DependencyPropertyBase
            .Register<CanvasUserControl, IObservable<BoundingBox>>(nameof(SelectionBorder), (canvas, value) =>
            {
                canvas.SelectionBorderDisposable?.Dispose();
                canvas.SelectionBorderDisposable = value.Subscribe(canvas.selectionBorder.CreateObserver(canvas.InteractiveCanvas));
            });

        public static readonly DependencyProperty EnablePanProperty = DependencyPropertyBase
            .Register<CanvasUserControl, bool>(nameof(EnablePan), (canvas, pan) =>
            {
                canvas.PanEnabledDisposable?.Dispose();
                canvas.ZoomEnabledDisposable?.Dispose();

                if (pan)
                {
                    canvas.PanEnabledDisposable = canvas.InteractiveCanvas.EnablePan();
                    canvas.ZoomEnabledDisposable = canvas.InteractiveCanvas.EnableZoom();
                }

            }, true);

        public static readonly DependencyProperty InvalidatorProperty = DependencyPropertyBase
            .Register<CanvasUserControl, INotifyEntityChanged<IUniqueScoreElement>>(nameof(Invalidator), (canvas, newval) =>
            {
                canvas.InvalidatorDisposable?.Dispose();
                canvas.InvalidatorDisposable = newval.Subscribe(canvas.EntityObserver);
            });

        public static readonly DependencyProperty PipeProperty = DependencyPropertyBase
            .Register<CanvasUserControl, IPipe>(nameof(Pipe), (canvas, newval) =>
            {
                canvas.PipeDisposable?.Dispose();
                canvas.PipeDisposable = canvas.InteractiveCanvas.Subscribe(newval);
            });



        public SceneManager<IUniqueScoreElement, int>? SceneManager
        {
            get => (SceneManager<IUniqueScoreElement, int>)GetValue(SceneProperty);
            set => SetValue(SceneProperty, value);
        }
        public IObservable<BoundingBox>? SelectionBorder
        {
            get => (IObservable<BoundingBox>)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }
        public bool EnablePan
        {
            get => (bool)GetValue(EnablePanProperty);
            set => SetValue(EnablePanProperty, value);
        }
        public INotifyEntityChanged<IUniqueScoreElement> Invalidator
        {
            get => (INotifyEntityChanged<IUniqueScoreElement>)GetValue(InvalidatorProperty);
            set => SetValue(InvalidatorProperty, value);
        }
        public IPipe Pipe
        {
            get => (IPipe)GetValue(PipeProperty);
            set => SetValue(PipeProperty, value);
        }



        public IDisposable? PanEnabledDisposable { get; set; }
        public IDisposable? ZoomEnabledDisposable { get; set; }
        public IDisposable? InvalidatorDisposable { get; set; }
        public IDisposable? SelectionBorderDisposable { get; set; }
        public IDisposable? PipeDisposable { get; set; }
        public BaseBitmapPainter BaseBitmapPainter { get; }
        public IObserver<IUniqueScoreElement> EntityObserver { get; }
        public IInteractiveCanvas InteractiveCanvas => canvas;



        public CanvasUserControl()
        {
            InitializeComponent();

            BaseBitmapPainter = new WindowsDrawingContextBitmapPainter(canvas);

            PanEnabledDisposable = canvas.EnablePan();
            ZoomEnabledDisposable = canvas.EnableZoom();

            EntityObserver = new EntityObserver(() => SceneManager, BaseBitmapPainter);
        }
    }
}
