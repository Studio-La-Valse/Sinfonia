using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.Drawable;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.StyleTemplates;

namespace Sinfonia.ViewModels.Application.Document
{
    public class CanvasViewModel : BaseViewModel
    {
        private readonly IUnitToPixelConverter unitToPixelConverter;
        private bool isInitialized = false;

        public ObservableBoundingBox SelectionBorder
        {
            get => GetValue(() => SelectionBorder);
            set => SetValue(() => SelectionBorder, value);
        }
        public INotifyEntityChanged<IUniqueScoreElement> Invalidator
        {
            get => GetValue(() => Invalidator);
            set => SetValue(() => Invalidator, value);
        }
        public SceneManager<IUniqueScoreElement, int> SceneManager
        {
            get => GetValue(() => SceneManager);
            set => SetValue(() => SceneManager, value);
        }

        public bool EnablePan
        {
            get => GetValue(() => EnablePan);
            set => SetValue(() => EnablePan, value);
        }

        public IPipe Pipe
        {
            get => GetValue(() => Pipe);
            set => SetValue(() => Pipe, value);
        }
        public double Zoom
        {
            get => GetValue(() => Zoom);
            set => SetValue(() => Zoom, value);
        }
        public double TranslateX
        {
            get => GetValue(() => TranslateX);
            set => SetValue(() => TranslateX, value);
        }
        public double TranslateY
        {
            get => GetValue(() => TranslateY);
            set => SetValue(() => TranslateY, value);
        }
        public Avalonia.Rect Bounds
        {
            get => GetValue(() => Bounds);
            set
            {
                SetValue(() => Bounds, value);

                // gross, but okay
                if (value != default)
                {
                    if (!isInitialized)
                    {
                        ZoomFirstPage();
                        isInitialized = true;
                    }
                }
            }
        }
        public ScoreDocumentStyleTemplate ScoreDocumentStyle { get; }
        public ISelectionManager<IUniqueScoreElement> Selection { get; }
        public IScoreDocument ScoreDocument { get; }
        public IVisualPageFactory VisualPageFactory { get; }


        public CanvasViewModel(INotifyEntityChanged<IUniqueScoreElement> observable,
                               ISelectionManager<IUniqueScoreElement> selection,
                               IScoreDocument scoreDocument,
                               IVisualPageFactory visualPageFactory,
                               ICommandManager commandManager,
                               IUnitToPixelConverter unitToPixelConverter,
                               SceneManager<IUniqueScoreElement, int> sceneManager,
                               ObservableBoundingBox observableBoundingBox,
                               ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            this.unitToPixelConverter = unitToPixelConverter;

            Invalidator = observable;
            Selection = selection;
            ScoreDocument = scoreDocument;
            VisualPageFactory = visualPageFactory;
            EnablePan = true;
            SceneManager = sceneManager;
            SelectionBorder = observableBoundingBox;
            ScoreDocumentStyle = scoreDocumentStyleTemplate;
            Pipe = Pipeline.DoNothing()
                .InterceptKeys(selection, out var _selectionManager)
                .ThenHandleDefaultMouseInteraction(SceneManager.VisualParents, Invalidator)
                .ThenHandleMouseHover(SceneManager.VisualParents, Invalidator)
                .ThenHandleDefaultClick(SceneManager.VisualParents, _selectionManager)
                .ThenHandleSelectionBorder(SceneManager.VisualParents, _selectionManager, SelectionBorder, Invalidator)
                .ThenHandleTransformations(_selectionManager, SceneManager.VisualParents, Invalidator)
                .ThenRender(Invalidator)
                .UndoRedo(commandManager, observable);

            Zoom = 1;
        }

        public void Rerender()
        {
            Invalidator.Invalidate(ScoreDocument);
            Invalidator.RenderChanges();
        }

        public void ZoomFirstPage()
        {
            var pageSize = PageSize.A4;
            var padding = 30;
            var desiredHeight = unitToPixelConverter.UnitsToPixels(pageSize.Height) + (padding * 2);
            var desiredZoom = Bounds.Height / desiredHeight;
            if(!double.IsNormal(desiredZoom))
            {
                return;
            }
            TranslateX = padding;
            TranslateY = padding;
            Zoom = desiredZoom;
        }
    }
}
