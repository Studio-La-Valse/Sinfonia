using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.ViewModels.Application.Document
{
    public class CanvasViewModel : BaseViewModel
    {
        private readonly IScoreDocumentReader scoreDocumentReader;

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
        public SceneManager<IUniqueScoreElement, int>? Scene
        {
            get => GetValue(() => Scene);
            set => SetValue(() => Scene, value);
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


        public ScoreDocumentStyleTemplate ScoreDocumentStyle { get; }
        public ISelectionManager<IUniqueScoreElement> Selection { get; }


        public CanvasViewModel(INotifyEntityChanged<IUniqueScoreElement> observable, IScoreDocumentReader scoreDocumentReader, ISelectionManager<IUniqueScoreElement> selection, SceneManager<IUniqueScoreElement, int> sceneManager, ObservableBoundingBox observableBoundingBox, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            Invalidator = observable;
            this.scoreDocumentReader = scoreDocumentReader;
            Selection = selection;
            EnablePan = true;
            Scene = sceneManager;
            SelectionBorder = observableBoundingBox;
            ScoreDocumentStyle = scoreDocumentStyleTemplate;
            Pipe = Pipeline.DoNothing()
                .InterceptKeys(selection, out ISelectionManager<IUniqueScoreElement>? _selectionManager)
                .ThenHandleDefaultMouseInteraction(Scene.VisualParents, Invalidator)
                .ThenHandleMouseHover(Scene.VisualParents, Invalidator)
                .ThenHandleDefaultClick(Scene.VisualParents, _selectionManager)
                .ThenHandleSelectionBorder(Scene.VisualParents, _selectionManager, SelectionBorder, Invalidator)
                .ThenHandleTransformations(_selectionManager, Scene.VisualParents, Invalidator)
                .ThenRender(Invalidator); ;
        }

        public void Rerender()
        {
            Invalidator.Invalidate(scoreDocumentReader);
            Invalidator.RenderChanges();
        }
    }
}
