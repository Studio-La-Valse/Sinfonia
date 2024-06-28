namespace Sinfonia.Implementations.Addin
{
    internal class AddinDocumentUI : IDocumentUI
    {
        private readonly CanvasViewModel canvasViewModel;

        public INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator => canvasViewModel.Invalidator;

        public AddinDocumentUI(CanvasViewModel canvasViewModel)
        {
            this.canvasViewModel = canvasViewModel;
        }

        public void RebuildScene()
        {
            canvasViewModel.Invalidator.Invalidate(canvasViewModel.ScoreDocument);
            canvasViewModel.Invalidator.RenderChanges();
        }
    }
}
