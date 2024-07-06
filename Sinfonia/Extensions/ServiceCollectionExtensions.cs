using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.Implementations.Addin;
using Sinfonia.Implementations.PDF;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Drawable;
using StudioLaValse.ScoreDocument.StyleTemplates;

namespace Sinfonia.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            return services.AddSingleton<IShellMethods, ShellMethods>()
                .AddSingleton(s => ScoreDocumentStyleTemplate.Create())
                .AddSingleton<IKeyGeneratorFactory<int>, IncrementalIntGeneratorFactory>()
                .AddTransient<IScoreStyleTemplateSaveService, ScoreStyleTemplateSaveService>()
                .AddTransient<IMusicXmlImportService, MusicXmlImportService>()
                .AddSingleton<IUnitToPixelConverter, MmToPixelConverter>()
                .AddSingleton<IPdfExportService, PdfExportService>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentViewModelFactory, DocumentViewModelFactory>()
                .AddSingleton<ICommandFactory, CommandFactory>()
                .AddSingleton<ImportMenuViewModel>()
                .AddSingleton<FileMenuViewModel>()
                .AddSingleton<ViewMenuViewModel>()
                .AddSingleton<MenuViewModel>()
                .AddSingleton<DocumentCollectionViewModel>();

            // Style template editor.
            services.AddSingleton<ScoreDocumentStyleTemplateViewModel>()
                .AddSingleton<PageStyleTemplateViewModel>()
                .AddSingleton<StaffSystemStyleTemplateViewModel>()
                .AddSingleton<StaffGroupStyleTemplateViewModel>()
                .AddSingleton<StaffStyleTemplateViewModel>()
                .AddSingleton<ScoreMeasureStyleTemplateViewModel>()
                .AddSingleton<InstrumentRibbonStyleTemplateViewModel>()
                .AddSingleton<InstrumentMeasureStyleTemplateViewModel>()
                .AddSingleton<MeasureBlockStyleTemplateViewModel>()
                .AddSingleton<ChordStyleTemplateViewModel>()
                .AddSingleton<NoteStyleTemplateViewModel>()
                .AddSingleton<DocumentStyleEditorViewModel>();

            services.AddSingleton<MainViewModel>()
                .AddSingleton<SideBarViewModel>()
                .AddSingleton<UserAccountViewModel>();

            return services;
        }

        public static IServiceCollection AddPersistence(this IServiceCollection services)
        {
            return services
                .AddSingleton<IFileSaveService, FileSaveService>();
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            return services.AddSingleton<MainWindow>()
                .AddSingleton<IOptionsWindowService, OptionsWindowService>();
        }

        public static IServiceCollection RegisterExternalAddins(this IServiceCollection services)
        {
            return services.AddSingleton<IApplication, AddinApplication>();
        }
    }
}
