using Microsoft.Win32;

namespace Sinfonia.Implementations
{
    internal class SaveFile : ISaveFile
    {
        public bool SaveToFile(string fileName, string extension, string filter, out string filePath)
        {
            filePath = "";

            SaveFileDialog openFileDialog = new()
            {
                FileName = fileName,
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
