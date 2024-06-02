using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.Implementations.Addin;
using Sinfonia.Implementations.PDF;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.Windows;

namespace Sinfonia.Extensions
{

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            return services.AddSingleton<IShellMethods, ShellMethods>()
                .AddSingleton<IKeyGeneratorFactory<int>, IncrementalIntGeneratorFactory>()
                .AddTransient<IScoreStyleTemplateSaveService, ScoreStyleTemplateSaveService>()
                .AddTransient<IMusicXmlImportService, MusicXmlImportService>()
                .AddSingleton<IPdfExportService, PdfExportService>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services.AddSingleton<IDocumentViewModelFactory, DocumentViewModelFactory>()
                .AddSingleton<ICommandFactory, CommandFactory>()
                .AddSingleton<ImportMenuViewModel>()
                .AddSingleton<FileMenuViewModel>()
                .AddSingleton<MenuViewModel>()
                .AddSingleton<DocumentCollectionViewModel>()
                .AddSingleton<MainViewModel>();
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
