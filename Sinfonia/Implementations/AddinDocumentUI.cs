namespace Sinfonia.Implementations
{
    internal class AddinDocumentUI : IDocumentUI
    {
        private readonly CanvasViewModel canvasViewModel;
        private readonly IScoreDocumentReader scoreDocumentReader;

        public INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator => canvasViewModel.Invalidator;

        public AddinDocumentUI(CanvasViewModel canvasViewModel, IScoreDocumentReader scoreDocumentReader)
        {
            this.canvasViewModel = canvasViewModel;
            this.scoreDocumentReader = scoreDocumentReader;
        }

        public void RebuildScene()
        {
            canvasViewModel.Invalidator.Invalidate(scoreDocumentReader);
            canvasViewModel.Invalidator.RenderChanges();
        }
    }
}
