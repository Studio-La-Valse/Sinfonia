namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class InstrumentRibbonLayoutMemento
    {
        public required string? AbbreviatedName { get; init; }
        public required string? DisplayName { get; init; }
        public required int? NumberOfStaves { get; init; }
        public required bool? Collapsed { get; init; }
    }
}