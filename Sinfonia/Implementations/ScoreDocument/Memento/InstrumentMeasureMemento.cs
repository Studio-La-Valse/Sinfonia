using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class InstrumentMeasureMemento
    {
        public required Guid Guid { get; init; }
        public required IInstrumentMeasureLayout Layout { get; init; }
        public required int MeasureIndex { get; init; }
        public required int RibbonIndex { get; init; }
        public required IEnumerable<RibbonMeasureVoiceMemento> VoiceGroups { get; init; }
    }
}
