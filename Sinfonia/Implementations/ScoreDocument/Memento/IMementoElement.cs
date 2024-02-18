namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public interface IMementoElement<TMemento>
    {
        Guid Guid { get; }
        TMemento GetMemento();
        void ApplyMemento(TMemento memento);
    }
}
