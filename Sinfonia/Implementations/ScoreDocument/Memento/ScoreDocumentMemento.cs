using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreDocumentMemento
    {
        public required Guid Guid { get; init; }
        public required ScoreDocumentLayoutMemento Layout { get; init; }
        public required IList<InstrumentRibbonMemento> InstrumentRibbons { get; init; }
        public required IList<ScoreMeasureMemento> ScoreMeasures { get; init; }
    }
}
