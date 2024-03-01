using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary;

namespace Sinfonia.ViewModels.Application.Document
{
    public class DocumentViewModel : BaseViewModel
    {
        private readonly IScoreLayoutDictionary scoreLayoutProvider;

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
        public SceneViewModel? SelectedScene
        {
            get => GetValue(() => SelectedScene);
            set
            {
                if (value is not null)
                {
                    var visualScene = value.ScoreDocumentScene.CreateScene(ScoreDocumentReader, scoreLayoutProvider);
                    var sceneManager = new SceneManager<IUniqueScoreElement, int>(visualScene, (e) => e.Id).WithBackground(ColorARGB.Black);
                    CanvasViewModel.Scene = sceneManager;
                }

                SetValue(() => SelectedScene, value);
            }
        }
        public ObservableCollection<SceneViewModel> AvailableScenes
        {
            get => GetValue(() => AvailableScenes);
            set => SetValue(() => AvailableScenes, value);
        }



        public IScoreBuilder ScoreBuilder { get; }
        public IScoreDocumentReader ScoreDocumentReader { get; }
        public ICommand ActivateSceneCommand { get; }
        public CanvasViewModel CanvasViewModel { get; }
        public ISelection<IUniqueScoreElement> Selection { get; }
        public ExplorerViewModel Explorer { get; }
        public InspectorViewModel Inspector { get; }



        internal DocumentViewModel(CanvasViewModel canvasViewModel, IEnumerable<SceneViewModel> availableScenes, ISelection<IUniqueScoreElement> selection, ICommandFactory commandFactory, IScoreBuilder scoreDocumentEditor, IScoreDocumentReader scoreDocumentReader, IScoreLayoutDictionary scoreLayoutProvider, ExplorerViewModel explorerViewModel, InspectorViewModel inspectorViewModel)
        {
            this.scoreLayoutProvider = scoreLayoutProvider;

            Selection = selection;
            CanvasViewModel = canvasViewModel;
            AvailableScenes = new ObservableCollection<SceneViewModel>(availableScenes);
            Header = "Unsaved document";
            ActivateSceneCommand = commandFactory.Create<SceneViewModel>(ActivateScene, (scene) => SelectedScene is null || scene.Name != SelectedScene.Name);
            ScoreBuilder = scoreDocumentEditor;
            ScoreDocumentReader = scoreDocumentReader;
            Explorer = explorerViewModel;
            Inspector = inspectorViewModel;
        }



        public void ActivateScene(SceneViewModel scene)
        {
            foreach (var _scene in AvailableScenes)
            {
                _scene.IsActive = false;
            }

            SelectedScene = scene;
            SelectedScene.IsActive = true;
        }

        public void SetActive()
        {
            if (!CanActivate)
            {
                return;
            }

            IsActive = true;
            CanActivate = false;
        }

        public void SetInactive()
        {
            IsActive = false;
            CanActivate = true;
        }
    }
}
