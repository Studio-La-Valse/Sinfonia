using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.Implementations.ScoreDocument;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Menu;
using StudioLaValse.Drawable.WPF.Commands;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            return services.AddSingleton<IShellMethods, ShellMethods>()
                .AddSingleton<IKeyGeneratorFactory<int>, IncrementalIntGeneratorFactory>()
                .AddSingleton<IScoreBuilderFactory, EmptyScoreBuilderFactory>()
                .AddTransient<IYamlConverter, YamlConverter>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services.AddSingleton<IDocumentViewModelFactory, DocumentViewModelFactory>()
                .AddSingleton<ICommandFactory, CommandFactory>()
                .AddSingleton<ImportMenuViewModel>()
                .AddSingleton<FileMenuViewModel>()
                .AddSingleton<ViewMenuViewModel>()
                .AddSingleton<MenuViewModel>()
                .AddSingleton<DocumentCollectionViewModel>()
                .AddSingleton<MainViewModel>();
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            return services.AddSingleton<MainWindow>()
                .AddScoped<IBrowseToFile, FileBrowser>()
                .AddScoped<ISaveFile, SaveFile>();
        }

        public static IServiceCollection RegisterExternalAddins(this IServiceCollection services)
        {
            return services.AddSingleton<IApplication, AddinApplication>();
        }
    }
}
