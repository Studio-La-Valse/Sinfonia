namespace Sinfonia.Implementations
{
    internal class AddinDocument : IDocument
    {
        private readonly DocumentViewModel documentViewModel;

        public IScoreBuilder ScoreBuilder => documentViewModel.ScoreBuilder;
        public ISelection<IUniqueScoreElement> Selection => documentViewModel.Selection;
        public INotifyEntityChanged<IUniqueScoreElement> EntityInvalidator => documentViewModel.CanvasViewModel.Invalidator;
        public IDocumentUI DocumentUI => new AddinDocumentUI(documentViewModel.CanvasViewModel, documentViewModel.ScoreDocumentReader);


        public AddinDocument(DocumentViewModel documentViewModel)
        {
            this.documentViewModel = documentViewModel;
        }
    }
}
