namespace Sinfonia.ViewModels.Application.Document.Explorer
{
    public class ExplorerViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
        private readonly IScoreDocumentReader scoreDocument;
        private readonly Queue<IUniqueScoreElement> queue = [];

        public ScoreElementViewModel ScoreDocument
        {
            get => GetValue(() => ScoreDocument);
            set => SetValue(() => ScoreDocument, value);
        }

        public ICommand RebuildCommand
        {
            get => GetValue(() => RebuildCommand);
            set => SetValue(() => RebuildCommand, value);
        }

        public ExplorerViewModel(IScoreDocumentReader scoreDocument, ScoreElementViewModel scoreDocumentViewModel, ICommandFactory commandFactory)
        {
            this.scoreDocument = scoreDocument;
            ScoreDocument = scoreDocumentViewModel;
            RebuildCommand = commandFactory.Create(Rebuild, () => true);
        }

        public void Rebuild()
        {
            ScoreDocument.Rebuild();
        }

        public void OnCompleted()
        {
            while (queue.Count > 0)
            {
                IUniqueScoreElement element = queue.Dequeue();
                IEnumerable<ScoreElementViewModel> viewModels = ScoreDocument.SelectRecursive(c => c.ScoreElements).Where(c => c.UniqueScoreElement.Equals(element));
                foreach (ScoreElementViewModel? viewModel in viewModels)
                {
                    viewModel.Rebuild();
                }
            }
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IUniqueScoreElement value)
        {
            queue.Enqueue(value);
        }
    }
}
