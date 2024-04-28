namespace Sinfonia.Interfaces
{
    public interface IDocumentViewModelFactory
    {
        DocumentViewModel Create(ScoreDocumentMemento scoreDocument);
    }
}
