namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class InstrumentMeasureLayoutMemento
    {
        public required IReadOnlySet<ClefChange> ClefChanges { get; init; }
        public required IReadOnlyDictionary<int, double> StaffPaddingBottom { get; init; }
        public required int? NumberofStaves { get; init; }
        public required double? PaddingBottom { get; init; }
        public required bool Collapsed { get; init; }
    }
}