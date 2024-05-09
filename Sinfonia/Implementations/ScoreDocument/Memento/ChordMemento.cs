using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ChordMemento
    {
        public required Guid Id { get; init; }
        public required ChordLayoutMemento Layout { get; init; }
        public required IList<NoteMemento> Notes { get; init; }
        public required RythmicDuration RythmicDuration { get; init; }
    }
}
