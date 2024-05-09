using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class MeasureBlockMemento
    {
        public required IList<ChordMemento> Chords { get; init; }
        public required RythmicDuration Duration { get; init; }
        public required Guid Id { get; init; }
        public required MeasureBlockLayoutMemento Layout { get; init; }
        public required int Voice { get;init; }
    }
}
