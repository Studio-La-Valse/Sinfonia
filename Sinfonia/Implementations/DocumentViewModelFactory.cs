﻿using CommandManager = StudioLaValse.CommandManager.CommandManager;

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
            var notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();

            var commandManager = CommandManager.CreateGreedy();
            var keyGenerator = keyGeneratorFactory.CreateKeyGenerator();

            var (scoreBuilder, reader, layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged);

            var inspectorViewModel = new InspectorViewModel(scoreBuilder, layout);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            notifyEntityChanged.Subscribe(inspectorViewModel);

            var selectionManager = SelectionManager<IUniqueScoreElement>.CreateDefault(e => e.Id)
                .AddChangedHandler(inspectorViewModel.Update, e => e.Id)
                .OnChangedNotify(notifyEntityChanged, e => e.Id);

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

            var explorerViewModel = new ExplorerViewModel(reader, scoreDocumentViewModel, commandFactory);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            notifyEntityChanged.Subscribe(explorerViewModel);

            var documentViewModel = new DocumentViewModel(canvasViewModel, scenes, selectionManager, commandFactory, scoreBuilder, reader, layout, explorerViewModel, inspectorViewModel);
            return documentViewModel;
        }
    }
}
