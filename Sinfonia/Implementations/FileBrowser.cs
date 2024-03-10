using Microsoft.Win32;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.Implementations
{
    internal class FileBrowser : IBrowseToFile
    {
        public bool BrowseToFile(out string filePath)
        {
            filePath = "";

            OpenFileDialog openFileDialog = new();

            if (openFileDialog.ShowDialog() == true)
            {
                filePath = openFileDialog.FileName;
                return true;
            }

            return false;
        }
    }
}
