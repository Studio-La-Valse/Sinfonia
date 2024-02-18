using CommandManager = StudioLaValse.CommandManager.CommandManager;

namespace Sinfonia.Implementations
{
    internal class DocumentViewModelFactory : IDocumentViewModelFactory
    {
        private readonly IEnumerable<IExternalScene> availableScenes;
        private readonly ICommandFactory commandFactory;

        public DocumentViewModelFactory(IAddinCollection<IExternalScene> availableScenes, ICommandFactory commandFactory)
        {
            this.availableScenes = availableScenes;
            this.commandFactory = commandFactory;
        }

        public DocumentViewModel Create(IScoreBuilderFactory scoreBuilderFactory)
        {
            var notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();

            var commandManager = CommandManager.CreateGreedy();

            var (scoreBuilder, reader) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged);

            var commandFactory = new ScoreCommandFactory(notifyEntityChanged);
            var inspectorViewModel = new InspectorViewModel(scoreBuilder);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            notifyEntityChanged.Subscribe(inspectorViewModel);

            var selectionManager = SelectionManager<IUniqueScoreElement>.CreateDefault()
                .AddChangedHandler(inspectorViewModel.Update)
                .OnChangedNotify(notifyEntityChanged);

            var scenes = availableScenes.Select(_scene =>
            {
                var settingsManagerViewModel = new PropertyManagerViewModel();
                var settingsManager = new AddinSettingsManager(settingsManagerViewModel);
                _scene.RegisterSettings(settingsManager);

                var sceneViewModel = new SceneViewModel(_scene, settingsManagerViewModel);
                return sceneViewModel;
            });

            var sceneSettingsManager = new PropertyManagerViewModel();
            var selectionBorder = new ObservableBoundingBox();
            var canvasViewModel = new CanvasViewModel(notifyEntityChanged, selectionManager, selectionBorder);

            var scoreDocumentViewModel = new ScoreElementViewModel(reader);
            scoreDocumentViewModel.Rebuild();

            var explorerViewModel = new ExplorerViewModel(reader, scoreDocumentViewModel);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            notifyEntityChanged.Subscribe(explorerViewModel);

            var documentViewModel = new DocumentViewModel(canvasViewModel, scenes, selectionManager, this.commandFactory, scoreBuilder, reader, explorerViewModel, inspectorViewModel);
            return documentViewModel;
        }
    }
}
