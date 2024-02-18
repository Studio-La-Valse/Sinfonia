using Microsoft.Extensions.DependencyInjection;
using Sinfonia.Implementations;
using Sinfonia.ViewModels.Application;
using Sinfonia.ViewModels.Menu;
using StudioLaValse.DependencyInjection.Microsoft;
using StudioLaValse.Drawable.WPF.Commands;
using System.IO;
using System.Reflection;

namespace Sinfonia.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddModels(this IServiceCollection services)
        {
            services.AddSingleton<IShellMethods, ShellMethods>();
            return services;
        }

        public static IServiceCollection AddViewModels(this IServiceCollection services)
        {
            services.AddSingleton<IDocumentViewModelFactory, DocumentViewModelFactory>();
            services.AddSingleton<ICommandFactory, CommandFactory>();
            services.AddSingleton<ImportMenuViewModel>();
            services.AddSingleton<FileMenuViewModel>();
            services.AddSingleton<MenuViewModel>();
            services.AddSingleton<DocumentCollectionViewModel>();
            services.AddSingleton<MainViewModel>();
            return services;
        }

        public static IServiceCollection AddViews(this IServiceCollection services)
        {
            services.AddSingleton<MainWindow>();
            services.AddScoped<Interfaces.IBrowseToFile, FileBrowser>();
            return services;
        }

        public static IServiceCollection RegisterExternalAddins(this IServiceCollection services)
        {
            services.AddSingleton<IApplication, AddinApplication>();
            return services;
        }

        public static IServiceCollection RegisterExternalScenes(this IServiceCollection services)
        {
            var directory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\External";
            var typeloader = TypeLoader.FromDirectory(directory);
            services.RegisterCollection<IExternalScene>();
            services.RegisterExternalAddins<IExternalScene>(typeloader, new ExternalSceneRegistryClient());
            return services;
        }
    }
}
