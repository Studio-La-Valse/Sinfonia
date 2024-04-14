using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Syncfusion.SfSkinManager;
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
            SfSkinManager.ApplyStylesOnApplication = true;

            var host = CreateHostBuilder(e.Args).Build();
            host.Start();

            var mainWindow = host.Services.GetRequiredService<MainWindow>();
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
