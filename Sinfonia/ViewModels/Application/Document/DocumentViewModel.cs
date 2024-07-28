using ReactiveUI;
using Sinfonia.ViewModels.Base;

namespace Sinfonia.ViewModels.Application.Document
{
    public class DocumentViewModel : BaseViewModel
    {
        public string Header
        {
            get => GetValue(() => Header);
            set => SetValue(() => Header, value);
        }


        public IScoreBuilder ScoreBuilder { get; }
        public IScoreDocument ScoreDocument { get; }
        public IKeyGenerator<int> KeyGenerator { get; }
        public ISelection<IUniqueScoreElement> Selection { get; }
        public ICommand CloseCommand { get; }
        public CanvasViewModel CanvasViewModel { get; }
        public ExplorerViewModel Explorer { get; }
        public InspectorViewModel Inspector { get; }

        public DocumentViewModel(DocumentCollectionViewModel documentCollectionViewModel,
                                 CanvasViewModel canvasViewModel,
                                 ExplorerViewModel explorerViewModel,
                                 InspectorViewModel inspectorViewModel,
                                 ISelection<IUniqueScoreElement> selection,
                                 IScoreBuilder scoreDocumentEditor,
                                 IScoreDocument scoreDocument,
                                 IKeyGenerator<int> keyGenerator)
        {
            Selection = selection;
            CanvasViewModel = canvasViewModel;
            Header = Guid.NewGuid().ToString();
            ScoreBuilder = scoreDocumentEditor;
            ScoreDocument = scoreDocument;
            Explorer = explorerViewModel;
            Inspector = inspectorViewModel;
            KeyGenerator = keyGenerator;

            CloseCommand = ReactiveCommand.Create<DocumentViewModel>(documentCollectionViewModel.Close);
        }
    }
}
