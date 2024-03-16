using Microsoft.Win32;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Implementations
{
    internal class FileBrowser : IBrowseToFile
    {
        public bool BrowseToFile(string extension, string filter, out string filePath)
        {
            filePath = "";

            OpenFileDialog openFileDialog = new()
            {
                DefaultExt = extension,
                Filter = filter
            };

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }
    }
}
