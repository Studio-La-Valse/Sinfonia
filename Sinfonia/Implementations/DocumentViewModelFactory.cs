using CommandManager = StudioLaValse.CommandManager.CommandManager;

namespace Sinfonia.Implementations
{
    internal class DocumentViewModelFactory : IDocumentViewModelFactory
    {
        private readonly IEnumerable<IExternalScene> availableScenes;
        private readonly ICommandFactory commandFactory;
        private readonly IKeyGeneratorFactory<int> keyGeneratorFactory;
        private readonly IScoreBuilderFactory scoreBuilderFactory;

        public DocumentViewModelFactory(IAddinCollection<IExternalScene> availableScenes, ICommandFactory commandFactory, IKeyGeneratorFactory<int> keyGeneratorFactory, IScoreBuilderFactory scoreBuilderFactory)
        {
            this.availableScenes = availableScenes;
            this.commandFactory = commandFactory;
            this.keyGeneratorFactory = keyGeneratorFactory;
            this.scoreBuilderFactory = scoreBuilderFactory;
        }

        public DocumentViewModel Create()
        {
            INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();

            ICommandManager commandManager = CommandManager.CreateGreedy();
            IKeyGenerator<int> keyGenerator = keyGeneratorFactory.CreateKeyGenerator();

            (IScoreBuilder scoreBuilder, IScoreDocumentReader reader, IScoreLayoutProvider layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged);

            InspectorViewModel inspectorViewModel = new(scoreBuilder, layout);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            _ = notifyEntityChanged.Subscribe(inspectorViewModel);

            ISelectionManager<IUniqueScoreElement> selectionManager = SelectionManager<IUniqueScoreElement>.CreateDefault(e => e.Id)
                .AddChangedHandler(inspectorViewModel.Update, e => e.Id)
                .OnChangedNotify(notifyEntityChanged, e => e.Id);

            IEnumerable<SceneViewModel> scenes = availableScenes.Select(_scene =>
            {
                SceneSettingsViewModel settingsManagerViewModel = new();
                AddinSettingsManager settingsManager = new(settingsManagerViewModel);
                _scene.RegisterSettings(settingsManager);

                SceneViewModel sceneViewModel = new(_scene, settingsManagerViewModel);
                return sceneViewModel;
            });

            ObservableBoundingBox selectionBorder = new();
            CanvasViewModel canvasViewModel = new(notifyEntityChanged, selectionManager, selectionBorder);

            ScoreElementViewModel scoreDocumentViewModel = new(reader);
            scoreDocumentViewModel.Rebuild();

            ExplorerViewModel explorerViewModel = new(reader, scoreDocumentViewModel, commandFactory);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            _ = notifyEntityChanged.Subscribe(explorerViewModel);

            DocumentViewModel documentViewModel = new(canvasViewModel, scenes, selectionManager, commandFactory, scoreBuilder, reader, layout, explorerViewModel, inspectorViewModel);
            return documentViewModel;
        }
    }
}
