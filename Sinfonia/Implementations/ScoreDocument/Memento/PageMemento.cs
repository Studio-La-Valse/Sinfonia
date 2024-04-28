namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class PageMemento
    {
        public required Guid Guid { get; init; }
        public required PageLayout? Layout { get; init; }
        public required int PageIndex { get; init; }
    }
}
