using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Sinfonia.Windows;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Sinfonia.Implementations;

internal class FileBrowser : IBrowseToFile
{
    private readonly MainWindow mainWindow;

    public FileBrowser(MainWindow mainWindow)
    {
        this.mainWindow = mainWindow;
    }

    public bool BrowseToFile(string extension, string filter, out string filePath)
    {
        filePath = "";

        var task = mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
        {
            Title = "Open",
            AllowMultiple = false,
        });
        var result = AsyncHelper.RunSync(() => task);

        if (result.Count == 0)
        {
            return false;
        }

        filePath = result[0].Path.LocalPath.ToString();
        return true;
    }
}

internal class SaveFile : ISaveFile
{
    private readonly MainWindow topLevel;

    public SaveFile(MainWindow topLevel)
    {
        this.topLevel = topLevel;
    }

    public bool SaveToFile(string fileName, string extension, string filter, out string filePath)
    {
        filePath = "";

        var task = topLevel.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save Text File"
        });

        var file = AsyncHelper.RunSync(() => task); 

        if(file is null)
        {
            return false;
        }

        filePath = file.Path.ToString();
        return true;
    }
}

internal static class AsyncHelper
{
    private static readonly TaskFactory _myTaskFactory = new
        TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

    public static TResult RunSync<TResult>(Func<Task<TResult>> func)
    {
        return _myTaskFactory
          .StartNew(func)
          .Unwrap()
          .GetAwaiter()
          .GetResult();
    }

    public static void RunSync(Func<Task> func)
    {
        _myTaskFactory
          .StartNew(func)
          .Unwrap()
          .GetAwaiter()
          .GetResult();
    }
}