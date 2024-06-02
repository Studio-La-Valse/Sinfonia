using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
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


        public ScoreDocumentStyleTemplate ScoreDocumentStyle { get; }
        public ISelectionManager<IUniqueScoreElement> Selection { get; }
        public IVisualPageFactory VisualPageFactory { get; }

        public CanvasViewModel(INotifyEntityChanged<IUniqueScoreElement> observable,
                               IScoreDocumentReader scoreDocumentReader,
                               ISelectionManager<IUniqueScoreElement> selection,
                               IVisualPageFactory visualPageFactory,
                               ICommandManager commandManager,
                               SceneManager<IUniqueScoreElement, int> sceneManager,
                               ObservableBoundingBox observableBoundingBox,
                               ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            this.scoreDocumentReader = scoreDocumentReader;

            Invalidator = observable;
            Selection = selection;
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
                .UndoRedo(commandManager);
        }

        public void Rerender()
        {
            Invalidator.Invalidate(scoreDocumentReader);
            Invalidator.RenderChanges();
        }
    }
}
