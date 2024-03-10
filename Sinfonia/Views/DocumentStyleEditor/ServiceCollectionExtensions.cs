using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.Implementations.ScoreDocument;
using Sinfonia.Views.DocumentStyleEditor.ViewModels;
using StudioLaValse.ScoreDocument.MusicXml;
using System.IO;
using System.Xml.Linq;
using CommandManager = StudioLaValse.CommandManager.CommandManager;
using MainViewModel = Sinfonia.Views.DocumentStyleEditor.ViewModels.MainViewModel;
using CanvasViewModel = Sinfonia.Views.DocumentStyleEditor.ViewModels.CanvasViewModel;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Views.DocumentStyleEditor
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDocumentStyleEditor(this IServiceCollection services)
        {
            return services.AddView().AddViewModels();
        }

        private static IServiceCollection AddView(this IServiceCollection services)
        {
            return services.AddTransient<DocumentStyleEditorView>()
                .AddTransient<DocumentStyleEditorViewFactory>()
                .AddSingleton<IDocumentStyleEditorLauncher, DocumentStyleEditorLauncher>();
        }

        private static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services
                .AddTransient<MainViewModel>()
                .AddSingleton(s =>
                {
                    var notifyPropertyChanged = SceneManager<IUniqueScoreElement, int>.CreateObservable();
                    var selection = SelectionManager<IUniqueScoreElement>.CreateDefault(e => e.Id);
                    var boundingBox = new ObservableBoundingBox();

                    var commandManager = CommandManager.CreateGreedy();
                    var style = new ScoreDocumentStyleTemplate();
                    var (builder, document, layout) = new EmptyScoreBuilderFactory(style).Create(commandManager, notifyPropertyChanged);

                    using (var ms = new MemoryStream(Resources.Kortjakje))
                    {
                        var xdocument = XDocument.Load(ms);
                        builder.BuildFromXml(xdocument);
                    };

                    var availableScenes = s.GetRequiredService<IAddinCollection<IExternalScene>>();
                    var scene = availableScenes.First().CreateScene(document, layout, selection);
                    var sceneManager = new SceneManager<IUniqueScoreElement, int>(scene, e => e.Id);
                    var canvasViewModel = new CanvasViewModel(notifyPropertyChanged, boundingBox, document, style)
                    {
                        Scene = sceneManager
                    };

                    return canvasViewModel;
                })
                .AddTransient<FileMenuViewModel>()
                .AddTransient<MenuViewModel>()
                .AddTransient<DocumentStyleEditorViewModel>()
                .AddTransient<PageViewModel>()
                .AddTransient<StaffSystemViewModel>()
                .AddTransient<StaffGroupViewModel>()
                .AddTransient<StaffViewModel>()
                .AddTransient<ScoreMeasureViewModel>()
                .AddTransient<InstrumentRibbonViewModel>()
                .AddTransient<InstrumentMeasureViewModel>()
                .AddTransient<MeasureBlockViewModel>()
                .AddTransient<ChordViewModel>()
                .AddTransient<NoteViewModel>();
        }
    }
}
