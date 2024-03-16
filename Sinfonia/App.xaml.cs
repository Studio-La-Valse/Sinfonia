using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
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
            IHost host = CreateHostBuilder(e.Args).Build();
            host.Start();

            MainWindow mainWindow = host.Services.GetRequiredService<MainWindow>();
            _ = mainWindow.ShowDialog();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureServices(services =>
                {
                    _ = services
                        .AddModels()
                        .AddViewModels()
                        .AddViews()
                        .RegisterExternalAddins();
                });
        }
    }
}
