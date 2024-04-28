namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class InstrumentRibbonMemento
    {
        public required Instrument Instrument { get; init; }
        public required Guid Guid { get; init; }
        public required IList<InstrumentMeasureMemento> InstrumentMeasures { get; init; }
        public required int IndexInScore { get; init; }
    }
}
