using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class NoteMemento
    {
        public required Pitch Pitch { get; init; }
        public required INoteLayout Layout { get; init; }
        public required Guid Guid { get; init; }
    }
}
