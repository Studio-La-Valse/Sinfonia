using Sinfonia.ViewModels.Application.Document.StyleTemplate;

namespace Sinfonia.ViewModels.Application.Document
{
    public class DocumentViewModel : BaseViewModel
    {
        public bool CanActivate
        {
            get => GetValue(() => CanActivate);
            set => SetValue(() => CanActivate, value);
        }
        public bool IsActive
        {
            get => GetValue(() => IsActive);
            set => SetValue(() => IsActive, value);
        }
        public string Header
        {
            get => GetValue(() => Header);
            set => SetValue(() => Header, value);
        }




        public IScoreBuilder ScoreBuilder { get; }
        public IScoreDocumentReader ScoreDocumentReader { get; }
        public IScoreDocumentLayout PageViewLayout { get; }
        public IKeyGenerator<int> KeyGenerator { get; }
        public CanvasViewModel CanvasViewModel { get; }
        public ISelection<IUniqueScoreElement> Selection { get; }
        public ExplorerViewModel Explorer { get; }
        public InspectorViewModel Inspector { get; }
        public DocumentStyleEditorViewModel DocumentStyleEditorViewModel { get; }

        internal DocumentViewModel(CanvasViewModel canvasViewModel, ExplorerViewModel explorerViewModel, InspectorViewModel inspectorViewModel, DocumentStyleEditorViewModel documentStyleEditorViewModel, ISelection<IUniqueScoreElement> selection, IScoreBuilder scoreDocumentEditor, IScoreDocumentReader scoreDocumentReader, IScoreDocumentLayout pageViewLayout, IKeyGenerator<int> keyGenerator)
        {
            Selection = selection;
            CanvasViewModel = canvasViewModel;
            Header = "Unsaved document";
            ScoreBuilder = scoreDocumentEditor;
            ScoreDocumentReader = scoreDocumentReader;
            PageViewLayout = pageViewLayout;
            Explorer = explorerViewModel;
            Inspector = inspectorViewModel;
            DocumentStyleEditorViewModel = documentStyleEditorViewModel;
            KeyGenerator = keyGenerator;
        }

        public void SetInactive()
        {
            IsActive = false;
            CanActivate = true;
        }

        public void SetActive()
        {
            IsActive = true;
            CanActivate = false;
        }
    }
}
