using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class ScoreDocumentMemento
    {
        public required Guid Id { get; init; }
        public required ScoreDocumentLayoutMemento Layout { get; set; }
        public required IList<InstrumentRibbonMemento> InstrumentRibbons { get; init; }
        public required IList<ScoreMeasureMemento> ScoreMeasures { get; set; }

        public static ScoreDocumentMemento Create()
        {
            var layout = ScoreDocumentLayoutMemento.Create();
            var memento = new ScoreDocumentMemento()
            {
                Id = Guid.NewGuid(),
                InstrumentRibbons = [],
                ScoreMeasures = [],
                Layout = layout
            };
            return memento;
        }
    }
}
