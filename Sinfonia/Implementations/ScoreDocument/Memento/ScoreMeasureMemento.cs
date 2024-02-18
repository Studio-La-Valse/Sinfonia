using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreMeasureMemento
    {
        public required IEnumerable<InstrumentMeasureMemento> Measures { get; init; }
        public required IScoreMeasureLayout Layout { get; init; }
        public required TimeSignature TimeSignature { get; init; }
        public required StaffSystemMemento StaffSystem { get; init; }
        public required Guid Guid { get; init; }
    }
}
