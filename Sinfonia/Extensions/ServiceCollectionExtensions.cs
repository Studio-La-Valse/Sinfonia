using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.Implementations.ScoreDocument;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Application.Menu;
using Sinfonia.Views.DocumentStyleEditor;
using StudioLaValse.DependencyInjection.Microsoft;
using StudioLaValse.Drawable.WPF.Commands;
using StudioLaValse.ScoreDocument.Layout.Templates;
using System.IO;
using System.Reflection;
using YamlDotNet.Serialization;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            return services.AddSingleton<IShellMethods, ShellMethods>()
                .AddSingleton<IKeyGeneratorFactory<int>, IncrementalIntGeneratorFactory>()
                .AddTransient<ScoreDocumentStyleTemplate>()
                .AddSingleton<IScoreBuilderFactory, EmptyScoreBuilderFactory>()
                .AddTransient<DeserializerBuilder>();
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
                .AddDocumentStyleEditor();
        }

        public static IServiceCollection RegisterExternalAddins(this IServiceCollection services)
        {
            return services.AddSingleton<IApplication, AddinApplication>();
        }

        public static IServiceCollection RegisterExternalScenes(this IServiceCollection services)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\External";
            var typeloader = TypeLoader.FromDirectory(directory);
            return services.RegisterCollection<IExternalScene>()
                .RegisterExternalAddins<IExternalScene>(typeloader, new ExternalSceneRegistryClient());
        }
    }
}
