using Sinfonia.Extensions;

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

        public ExplorerViewModel(IScoreDocumentReader scoreDocument, ScoreElementViewModel scoreDocumentViewModel)
        {
            this.scoreDocument = scoreDocument;
            ScoreDocument = scoreDocumentViewModel;
        }

        public void OnCompleted()
        {
            while (queue.Count > 0)
            {
                var element = queue.Dequeue();
                var viewModel = ScoreDocument.SelectRecursive(c => c.ScoreElements).FirstOrDefault(c => c.UniqueScoreElement == element);
                if (viewModel is null)
                {
                    continue;
                }

                viewModel.Rebuild();
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
