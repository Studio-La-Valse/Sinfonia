using Sinfonia.ViewModels.Application.Document.StyleTemplate;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.Layout.Templates;
using CommandManager = StudioLaValse.CommandManager.CommandManager;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Implementations
{
    internal class DocumentViewModelFactory : IDocumentViewModelFactory
    {
        private readonly ICommandFactory commandFactory;
        private readonly IKeyGeneratorFactory<int> keyGeneratorFactory;
        private readonly IScoreBuilderFactory scoreBuilderFactory;
        private readonly IBrowseToFile browseToFile;
        private readonly ISaveFile saveFile;
        private readonly IYamlConverter yamlConverter;

        public DocumentViewModelFactory(ICommandFactory commandFactory, IKeyGeneratorFactory<int> keyGeneratorFactory, IScoreBuilderFactory scoreBuilderFactory, IBrowseToFile browseToFile, ISaveFile saveFile, IYamlConverter yamlConverter)
        {
            this.commandFactory = commandFactory;
            this.keyGeneratorFactory = keyGeneratorFactory;
            this.scoreBuilderFactory = scoreBuilderFactory;
            this.browseToFile = browseToFile;
            this.saveFile = saveFile;
            this.yamlConverter = yamlConverter;
        }

        public DocumentViewModel Create()
        {
            INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();

            ICommandManager commandManager = CommandManager.CreateGreedy();
            IKeyGenerator<int> keyGenerator = keyGeneratorFactory.CreateKeyGenerator();

            var style = new ScoreDocumentStyleTemplate();
            (IScoreBuilder scoreBuilder, IScoreDocumentReader reader, IScoreDocumentLayout layout) = scoreBuilderFactory.Create(commandManager, notifyEntityChanged, style);

            ScoreElementViewModel scoreDocumentViewModel = new(reader);
            scoreDocumentViewModel.Rebuild();

            ExplorerViewModel explorerViewModel = new(reader, scoreDocumentViewModel, commandFactory);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            _ = notifyEntityChanged.Subscribe(explorerViewModel);

            InspectorViewModel inspectorViewModel = new(scoreBuilder, layout);
            // todo: UNSUBSCRIBE WHEN DOCUMENT CLOSES
            _ = notifyEntityChanged.Subscribe(inspectorViewModel);

            ISelectionManager<IUniqueScoreElement> selectionManager = SelectionManager<IUniqueScoreElement>.CreateDefault(e => e.Id)
                .AddChangedHandler(inspectorViewModel.Update, e => e.Id)
                .OnChangedNotify(notifyEntityChanged, e => e.Id);

            ObservableBoundingBox selectionBorder = new();

            VisualNoteFactory noteFactory = new(selectionManager, layout);
            VisualRestFactory restFactory = new(selectionManager);
            VisualNoteGroupFactory noteGroupFactory = new(noteFactory, restFactory, layout);
            VisualStaffMeasureFactory staffMeasusureFactory = new(selectionManager, noteGroupFactory, layout);
            VisualSystemMeasureFactory systemMeasureFactory = new(selectionManager, staffMeasusureFactory, layout);
            VisualStaffSystemFactory staffSystemFactory = new(systemMeasureFactory, selectionManager, layout);
            PageViewSceneFactory sceneFactory = new(staffSystemFactory, 20, 30, ColorARGB.Black, ColorARGB.White, layout);
            VisualScoreDocumentScene scene = new(sceneFactory, reader);
            SceneManager<IUniqueScoreElement, int> sceneManager = new(scene, e => e.Id);

            CanvasViewModel canvasViewModel = new(notifyEntityChanged, reader, selectionManager, sceneManager, selectionBorder, style);

            PageViewModel pageViewModel = new(canvasViewModel);
            StaffSystemViewModel staffSystemViewModel = new(canvasViewModel);
            StaffGroupViewModel staffGroupViewModel = new(canvasViewModel);
            StaffViewModel staffViewModel = new(canvasViewModel);
            ScoreMeasureViewModel scoreMeasureViewModel = new(canvasViewModel);
            InstrumentRibbonViewModel instrumentRibbonViewModel = new(canvasViewModel);
            InstrumentMeasureViewModel instrumentMeasureViewModel = new(canvasViewModel);
            MeasureBlockViewModel measureBlockViewModel = new(canvasViewModel);
            ChordViewModel chordViewModel = new(canvasViewModel);
            NoteViewModel noteViewModel = new(canvasViewModel);
            DocumentStyleEditorViewModel styleEditorViewModel = new(
                canvasViewModel,
                pageViewModel,
                staffSystemViewModel,
                staffGroupViewModel,
                staffViewModel,
                scoreMeasureViewModel,
                instrumentRibbonViewModel,
                instrumentMeasureViewModel,
                measureBlockViewModel,
                chordViewModel,
                noteViewModel,
                commandFactory,
                yamlConverter,
                browseToFile,
                saveFile);
            styleEditorViewModel.Rebuild();

            DocumentViewModel documentViewModel = new(canvasViewModel, explorerViewModel, inspectorViewModel, styleEditorViewModel, selectionManager, scoreBuilder, reader, layout, keyGenerator);
            return documentViewModel;
        }
    }
}
