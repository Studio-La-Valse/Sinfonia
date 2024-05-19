using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sinfonia.Implementations.ScoreDocument;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Document.StyleTemplate;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations
{
    public class DocumentViewModelFactory : IDocumentViewModelFactory
    {
        private readonly ICommandFactory commandFactory;
        private readonly IKeyGeneratorFactory<int> keyGeneratorFactory;
        private readonly IScoreStyleTemplateSaveService yamlConverter;
        private readonly DocumentCollectionViewModel documentCollectionViewModel;

        public DocumentViewModelFactory(ICommandFactory commandFactory,
                                        IKeyGeneratorFactory<int> keyGeneratorFactory,
                                        IScoreStyleTemplateSaveService yamlConverter,
                                        DocumentCollectionViewModel documentCollectionViewModel)
        {
            this.commandFactory = commandFactory;
            this.keyGeneratorFactory = keyGeneratorFactory;
            this.yamlConverter = yamlConverter;
            this.documentCollectionViewModel = documentCollectionViewModel;
        }

        public DocumentViewModel Create(ScoreDocumentModel scoreDocument)
        {
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services
                    .AddSingleton(documentCollectionViewModel)
                    .AddSingleton(commandFactory)
                    .AddSingleton(yamlConverter)
                    .AddSingleton(keyGeneratorFactory.CreateKeyGenerator())
                    .AddSingleton(CommandManager.CreateGreedy())
                    .AddScoreDocument(scoreDocument)
                    .AddSelection()
                    .AddScene()
                    .AddViewModels();
            });

            var host = hostBuilder.Build();

            var documentViewModel = host.Services.GetRequiredService<DocumentViewModel>();
            return documentViewModel;
        }
    }

    file static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSelection(this IServiceCollection services)
        {
            return services
                .AddSingleton(services =>
                {
                    var inspectorViewModel = services.GetRequiredService<InspectorViewModel>();
                    var notifyEntityChanged = services.GetRequiredService<INotifyEntityChanged<IUniqueScoreElement>>();
                    return SelectionManager<IUniqueScoreElement>.CreateDefault(e => e.Id)
                        .AddChangedHandler(inspectorViewModel.Update, e => e.Id)
                        .OnChangedNotify(notifyEntityChanged, e => e.Id);
                })
                .AddSingleton<ISelection<IUniqueScoreElement>>(services =>
                {
                    return services.GetRequiredService<ISelectionManager<IUniqueScoreElement>>();
                });
            
        }

        public static IServiceCollection AddScoreDocument(this IServiceCollection services, ScoreDocumentModel scoreDocumentMemento)
        {
            return services
                .AddSingleton<ScoreDocumentStyleTemplate>()
                .AddSingleton<InstrumentMeasureFactory>()
                .AddSingleton<ScoreContentTable>()
                .AddSingleton<PageGenerator>()
                .AddSingleton(services =>
                {
                    var contentTable = services.GetRequiredService<ScoreContentTable>();
                    var pageGenerator = services.GetRequiredService<PageGenerator>();
                    var styleTemplate = services.GetRequiredService<ScoreDocumentStyleTemplate>();
                    var keyGenerator = services.GetRequiredService<IKeyGenerator<int>>();

                    var layoutMemento = scoreDocumentMemento.Layout;
                    var primaryLayout = new PrimaryScoreDocumentLayout(styleTemplate);
                    var secondaryLayout = new SecondaryScoreDocumentLayout(primaryLayout, layoutMemento.Id);
                    var scoreDocument = new ScoreDocumentCore(contentTable, pageGenerator, styleTemplate, primaryLayout, secondaryLayout, keyGenerator, scoreDocumentMemento.Id);
                    scoreDocument.ApplyMemento(scoreDocumentMemento);

                    return scoreDocument;
                })
                .AddSingleton<IScoreDocumentLayout>(services => services.GetRequiredService<ScoreDocumentCore>().SecondaryLayout)
                .AddSingleton<IScoreBuilder, ScoreBuilder>()
                .AddSingleton<IScoreDocumentReader, ScoreDocumentReaderProxy>()
                .AddTransient<IScoreDocumentEditor, ScoreDocumentEditorProxy>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddSingleton<ScoreDocumentTreeViewViewModel>()
                .AddSingleton<ScoreElementViewModel>()
                .AddSingleton<ExplorerViewModel>()
                .AddSingleton<InspectorViewModel>()
                .AddSingleton<CanvasViewModel>()
                .AddSingleton<ScoreDocumentViewModel>()
                .AddSingleton<PageViewModel>()
                .AddSingleton<StaffSystemViewModel>()
                .AddSingleton<StaffGroupViewModel>()
                .AddSingleton<StaffViewModel>()
                .AddSingleton<ScoreMeasureViewModel>()
                .AddSingleton<InstrumentRibbonViewModel>()
                .AddSingleton<InstrumentMeasureViewModel>()
                .AddSingleton<MeasureBlockViewModel>()
                .AddSingleton<ChordViewModel>()
                .AddSingleton<NoteViewModel>()
                .AddSingleton<DocumentStyleEditorViewModel>()
                .AddSingleton<DocumentViewModel>();
        }

        public static IServiceCollection AddScene(this IServiceCollection services)
        {
            return services
                .AddSingleton<ObservableBoundingBox>()
                .AddSingleton<IVisualNoteFactory, VisualNoteFactory>()
                .AddSingleton<IVisualRestFactory, VisualRestFactory>()
                .AddSingleton<IVisualNoteGroupFactory, VisualNoteGroupFactory>()
                .AddSingleton<IVisualInstrumentMeasureFactory, VisualInstrumentMeasureFactory>()
                .AddSingleton<IVisualSystemMeasureFactory, VisualSystemMeasureFactory>()
                .AddSingleton<IVisualStaffSystemFactory, VisualStaffSystemFactory>()
                .AddSingleton<IVisualScoreDocumentContentFactory, PageViewSceneFactory>(services =>
                {
                    var staffSystemContentFactory = services.GetRequiredService<IVisualStaffSystemFactory>();
                    var layout = services.GetRequiredService<IScoreDocumentLayout>();
                    return new PageViewSceneFactory(staffSystemContentFactory, 20, 50, layout);
                })
                .AddSingleton<VisualScoreDocumentScene>()
                .AddSingleton(services =>
                {
                    var scene = services.GetRequiredService<VisualScoreDocumentScene>();
                    return new SceneManager<IUniqueScoreElement, int>(scene, e => e.Id).WithBackground(StudioLaValse.Geometry.ColorARGB.Transparant);
                })
                .AddSingleton(SceneManager<IUniqueScoreElement, int>.CreateObservable());
        }
    }
}
