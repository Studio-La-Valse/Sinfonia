namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreMeasureMemento
    {
        public required IEnumerable<InstrumentMeasureMemento> Measures { get; init; }
        public required TimeSignature TimeSignature { get; init; }
        public required Guid Guid { get; init; }
    }
}
