using Sinfonia.Implementations.ScoreDocument.Memento.Layout;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class InstrumentMeasureMemento
    {
        public required Guid Guid { get; init; }
        public required int MeasureIndex { get; init; }
        public required int RibbonIndex { get; init; }
        public required IEnumerable<RibbonMeasureVoiceMemento> VoiceGroups { get; init; }
        public required InstrumentMeasureLayoutMemento Layout { get; init; }
    }
}
