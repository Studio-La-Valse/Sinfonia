using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class NoteMemento
    {
        public required Pitch Pitch { get; init; }
        public required Guid Guid { get; init; }
        public required NoteLayoutMemento? Layout { get; init; }
    }
}
