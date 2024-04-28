namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ChordMemento
    {
        public required IList<NoteMemento> Notes { get; init; }
        public required RythmicDuration RythmicDuration { get; init; }
        public required Guid Guid { get; init; }
    }
}
