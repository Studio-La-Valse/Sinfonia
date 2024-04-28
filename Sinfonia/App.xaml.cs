using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sinfonia.EntityFramework;
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

            using var dbContextScope = host.Services.CreateScope();
            var dbContext = host.Services.GetRequiredService<ScoreDocumentContext>();
            dbContext.Database.EnsureCreated();

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
                        .AddDbContext()
                        .AddViewModels()
                        .AddViews()
                        .RegisterExternalAddins();
                });
        }
    }
}
