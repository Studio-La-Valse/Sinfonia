namespace Sinfonia.ViewModels.Application.Document
{
    public class CanvasViewModel : BaseViewModel
    {
        private readonly ISelectionManager<IUniqueScoreElement> selection;

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
            set
            {
                if (value is null)
                {
                    Pipe = Pipeline.DoNothing();
                    SetValue(() => Scene, value);
                    return;
                }

                Pipe = Pipeline.DoNothing()
                    .InterceptKeys(selection, out var _selectionManager)
                    .ThenHandleDefaultMouseInteraction(value.VisualParents, Invalidator)
                    .ThenHandleMouseHover(value.VisualParents, Invalidator)
                    .ThenHandleDefaultClick(value.VisualParents, _selectionManager)
                    .ThenHandleSelectionBorder(value.VisualParents, _selectionManager, SelectionBorder, Invalidator)
                    .ThenHandleTransformations(_selectionManager, value.VisualParents, Invalidator)
                    .ThenRender(Invalidator);

                SetValue(() => Scene, value);
            }
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

        public CanvasViewModel(INotifyEntityChanged<IUniqueScoreElement> observable, ISelectionManager<IUniqueScoreElement> selection, ObservableBoundingBox observableBoundingBox)
        {
            this.selection = selection;

            Invalidator = observable;
            EnablePan = true;
            SelectionBorder = observableBoundingBox;
            Pipe = Pipeline.DoNothing();
        }
    }
}
