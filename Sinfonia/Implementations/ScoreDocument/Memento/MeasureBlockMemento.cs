namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class MeasureBlockMemento
    {
        public required IList<ChordMemento> Chords { get; init; }
        public required RythmicDuration Duration { get; init; }
        public required bool Grace { get; init; }
        public required Guid Guid { get; init; }
    }
}
