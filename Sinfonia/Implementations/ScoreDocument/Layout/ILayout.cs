namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal interface ILayout<TMemento>
    {
        Guid Id { get; }
        TMemento GetMemento();
        void ApplyMemento(TMemento memento);
        void Restore();
    }
}