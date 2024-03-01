namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class InstrumentRibbonMemento
    {
        public required IEnumerable<InstrumentMeasureMemento> Measures { get; init; }
        public required Instrument Instrument { get; init; }
        public required Guid Guid { get; init; }
    }
}
