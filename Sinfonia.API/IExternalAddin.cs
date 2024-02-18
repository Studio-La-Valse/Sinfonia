namespace Sinfonia.API
{
    public interface IExternalAddin
    {
        string Name { get; }
        string Description { get; }
        string Author { get; }
        Guid Guid { get; }
    }
}