using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument;

namespace Sinfonia.ViewModels.Application.Document.Explorer
{
    public class ExplorerViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
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

        public ExplorerViewModel(ScoreDocumentTreeViewViewModel scoreDocumentViewModel, ICommandFactory commandFactory)
        {
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
                var element = queue.Dequeue();
                var viewModels = ScoreDocument.SelectRecursive(c => c.ScoreElements).Where(c => c.UniqueScoreElement.Equals(element));
                foreach (var viewModel in viewModels)
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

    public class ScoreDocumentTreeViewViewModel : ScoreElementViewModel
    {
        public ScoreDocumentTreeViewViewModel(IScoreDocumentReader scoreDocumentReader) : base(scoreDocumentReader)
        {
             
        }
    }
}
