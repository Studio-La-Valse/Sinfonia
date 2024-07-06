using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sinfonia.ViewModels.Application;
using StudioLaValse.ScoreDocument.Drawable;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.GlyphLibrary;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.StyleTemplates;
using StudioLaValse.ScoreDocument.Models;

namespace Sinfonia.Implementations
{
    public class DocumentViewModelFactory : IDocumentViewModelFactory
    {
        private readonly ICommandFactory commandFactory;
        private readonly IKeyGeneratorFactory<int> keyGeneratorFactory;
        private readonly IScoreStyleTemplateSaveService yamlConverter;
        private readonly IUnitToPixelConverter unitToPixelConverter;
        private readonly DocumentCollectionViewModel documentCollectionViewModel;
        private readonly ScoreDocumentStyleTemplate scoreDocumentStyleTemplate;

        public DocumentViewModelFactory(ICommandFactory commandFactory,
                                        IKeyGeneratorFactory<int> keyGeneratorFactory,
                                        IScoreStyleTemplateSaveService yamlConverter,
                                        IUnitToPixelConverter unitToPixelConverter,
                                        DocumentCollectionViewModel documentCollectionViewModel,
                                        ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            this.commandFactory = commandFactory;
            this.keyGeneratorFactory = keyGeneratorFactory;
            this.yamlConverter = yamlConverter;
            this.unitToPixelConverter = unitToPixelConverter;
            this.documentCollectionViewModel = documentCollectionViewModel;
            this.scoreDocumentStyleTemplate = scoreDocumentStyleTemplate;
        }

        public DocumentViewModel Create(ScoreDocumentModel scoreDocument)
        {
            var hostBuilder = Host.CreateDefaultBuilder().ConfigureServices(services =>
            {
                services
                    .AddSingleton(documentCollectionViewModel)
                    .AddSingleton(scoreDocumentStyleTemplate)
                    .AddSingleton(unitToPixelConverter)
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

        public static IServiceCollection AddScoreDocument(this IServiceCollection services, ScoreDocumentModel scoreDocumentModel)
        {
            return services
                .AddSingleton(services =>
                {
                    var styleTemplate = services.GetRequiredService<ScoreDocumentStyleTemplate>();
                    var commandManager = services.GetRequiredService<ICommandManager>();
                    var notifyChanged = services.GetRequiredService<INotifyEntityChanged<IUniqueScoreElement>>();
                    var scoreDocument = ScoreDocument.Create(styleTemplate, commandManager, notifyChanged, scoreDocumentModel);
                    return scoreDocument;
                })
                .AddSingleton(services =>
                {
                    var scoreDocument = services.GetRequiredService<IScoreDocument>();
                    var commandManager = services.GetRequiredService<ICommandManager>();
                    var notifyChanged = services.GetRequiredService<INotifyEntityChanged<IUniqueScoreElement>>();
                    var scoreBuilder = ScoreBuilder.Create(scoreDocument, commandManager, notifyChanged);
                    return scoreBuilder;
                });
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddSingleton<ScoreDocumentTreeViewViewModel>()
                .AddSingleton<ScoreElementViewModel>()
                .AddSingleton<ExplorerViewModel>()
                .AddSingleton<InspectorViewModel>()
                .AddSingleton<CanvasViewModel>()
                .AddSingleton<DocumentViewModel>();
        }

        public static IServiceCollection AddScene(this IServiceCollection services)
        {
            return services
                .AddSingleton<ObservableBoundingBox>()
                .AddSingleton<IGlyphLibrary, GenericGlyphLibrary>()
                .AddSingleton<IVisualNoteFactory, VisualNoteFactory>()
                .AddSingleton<IVisualRestFactory, VisualRestFactory>()
                .AddSingleton<IVisualNoteGroupFactory, VisualNoteGroupFactory>()
                .AddSingleton<IVisualInstrumentMeasureFactory, VisualInstrumentMeasureFactory>()
                .AddSingleton<IVisualSystemMeasureFactory, VisualSystemMeasureFactory>()
                .AddSingleton<IVisualStaffSystemFactory, VisualStaffSystemFactory>()
                .AddSingleton<IVisualPageFactory, VisualPageFactory>()
                .AddSingleton<IVisualScoreDocumentContentFactory, PageViewSceneFactory>()
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
