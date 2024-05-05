using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Sinfonia.EntityFramework;
using Sinfonia.Implementations;
using Sinfonia.Implementations.Addin;
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
                .AddTransient<IYamlConverter, YamlConverter>();
        }

        public static IServiceCollection AddDbContext(this IServiceCollection services)
        {
            return services
                .AddDbContext<ScoreDocumentContext>(options =>
                {
                    options.UseSqlite("DataSource=file::memory:?cache=shared");
                })
                .AddScoped<IScoreDocumentRepository, ScoreDocumentRepository>();
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            return services.AddSingleton<IDocumentViewModelFactory, DocumentViewModelFactory>()
                .AddSingleton<ICommandFactory, CommandFactory>()
                .AddSingleton<ImportMenuViewModel>()
                .AddSingleton<FileMenuViewModel>()
                .AddSingleton<MenuViewModel>()
                .AddSingleton<DocumentCollectionViewModel>()
                .AddSingleton<DocumentMenuViewModel>()
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
