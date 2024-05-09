namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class InstrumentMeasureLayoutMemento
    {
        public required Guid Id { get; init; }
        public required IReadOnlyCollection<ClefChange> ClefChanges { get; init; }
        public required IReadOnlyDictionary<int, double> StaffPaddingBottom { get; init; }
        public required int? NumberOfStaves { get; init; }
        public required bool? Collapsed { get; init; }
        public required double? PaddingBottom { get; init; }
    }
}