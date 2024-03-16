namespace Sinfonia.Interfaces
{
    public interface IBrowseToFile
    {
        bool BrowseToFile(string extension, string filter, out string filePath);
    }
}
