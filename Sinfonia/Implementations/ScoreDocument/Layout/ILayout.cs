namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal interface ILayout<TMemento> : IMementoElement<TMemento>
    {
        void Restore();
    }
}