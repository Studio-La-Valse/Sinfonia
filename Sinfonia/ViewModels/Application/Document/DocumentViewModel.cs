using ReactiveUI;
using Sinfonia.ViewModels.Application.Document.StyleTemplate;
using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.Reader;

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
        public IScoreDocumentReader ScoreDocumentReader { get; }
        public IKeyGenerator<int> KeyGenerator { get; }
        public CanvasViewModel CanvasViewModel { get; }
        public ISelection<IUniqueScoreElement> Selection { get; }
        public ExplorerViewModel Explorer { get; }
        public InspectorViewModel Inspector { get; }
        public DocumentStyleEditorViewModel DocumentStyleEditorViewModel { get; }
        public ICommand CloseCommand { get; }

        public DocumentViewModel(DocumentCollectionViewModel documentCollectionViewModel,
                                 CanvasViewModel canvasViewModel,
                                 ExplorerViewModel explorerViewModel,
                                 InspectorViewModel inspectorViewModel,
                                 DocumentStyleEditorViewModel documentStyleEditorViewModel,
                                 ISelection<IUniqueScoreElement> selection,
                                 IScoreBuilder scoreDocumentEditor,
                                 IScoreDocumentReader scoreDocumentReader,
                                 IKeyGenerator<int> keyGenerator)
        {
            Selection = selection;
            CanvasViewModel = canvasViewModel;
            Header = Guid.NewGuid().ToString();
            ScoreBuilder = scoreDocumentEditor;
            ScoreDocumentReader = scoreDocumentReader;
            Explorer = explorerViewModel;
            Inspector = inspectorViewModel;
            DocumentStyleEditorViewModel = documentStyleEditorViewModel;
            KeyGenerator = keyGenerator;

            CloseCommand = ReactiveCommand.Create<DocumentViewModel>(d => documentCollectionViewModel.Close(d));
        }
    }
}
