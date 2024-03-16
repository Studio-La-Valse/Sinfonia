namespace Sinfonia.Interfaces
{
    public interface ISaveFile
    {
        bool SaveToFile(string fileName, string extension, string filter, out string filePath);
    }
}
