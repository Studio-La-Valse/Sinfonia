using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sinfonia.ViewModels.Application;
using Sinfonia.Controls;
using Sinfonia.Windows;

namespace Sinfonia;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        using var host = CreateHostBuilder().Build();
        var mainWindow = host.Services.GetRequiredService<MainWindow>();
        var mainViewModel = host.Services.GetRequiredService<MainViewModel>();
        mainWindow.DataContext = mainViewModel;

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = mainWindow;
        }
        else if (ApplicationLifetime is ISingleViewApplicationLifetime singleViewPlatform)
        {
            singleViewPlatform.MainView = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    private static HostApplicationBuilder CreateHostBuilder()
    {
        var builder = Host.CreateApplicationBuilder(Environment.GetCommandLineArgs());
        builder.Services.AddModels().AddDbContext().AddViewModels().AddViews();
        return builder;
    }
}
