namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal interface ILayout<TMemento>
    {
        TMemento GetMemento();
        void ApplyMemento(TMemento memento);
        void Restore();
    }
}