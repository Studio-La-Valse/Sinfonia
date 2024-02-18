namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public interface ILayoutElement<TLayout>
    {
        Guid Guid { get; }
        TLayout ReadLayout();
        void ApplyLayout(TLayout memento);
    }
}
