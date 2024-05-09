using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Sinfonia.Windows;
using System.IO;

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
