using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreMeasureMemento
    {
        public required Guid Id { get; init; }
        public required ScoreMeasureLayoutMemento Layout { get; init; }
        public required IList<InstrumentMeasureMemento> InstrumentMeasures { get; init; }
        public required TimeSignature TimeSignature { get; init; }
        public required int IndexInScore { get; init; }
    }
}
