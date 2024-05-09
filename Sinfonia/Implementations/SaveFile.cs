using Avalonia.Platform.Storage;
using Sinfonia.Windows;

namespace Sinfonia.Implementations;

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
