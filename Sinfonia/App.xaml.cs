using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sinfonia.Extensions;
using System.Windows;

namespace Sinfonia
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            var host = CreateHostBuilder(e.Args).Build();
            host.Start();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
            mainWindow.ShowDialog();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    services
                        .AddModels()
                        .AddViewModels()
                        .AddViews()
                        .RegisterExternalAddins()
                        .RegisterExternalScenes();
                });
        }
    }
}
