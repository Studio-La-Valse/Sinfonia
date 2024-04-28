namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreMeasureMemento
    {
        public required Guid Guid { get; init; }
        public required IList<InstrumentMeasureMemento> Measures { get; init; }
        public required TimeSignature TimeSignature { get; init; }
        public required int IndexInScore { get; init; }
    }
}
