using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ChordMemento
    {
        public required IEnumerable<NoteMemento> Notes { get; init; }
        public required IChordLayout Layout { get; init; }
        public required RythmicDuration RythmicDuration { get; init; }
        public required Guid Guid { get; init; }
    }
}
