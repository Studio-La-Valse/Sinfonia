namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class ScoreDocumentLayoutMemento
    {
        required public double? Scale { get; init; }
        required public double? HorizontalStaffLineThickness { get; init; }
        required public double? VerticalStaffLineThickness { get; init; }
        required public double? StemLineThickness { get; init; }
        required public double? FirstSystemIndent { get; init; }
        required public ColorARGB? PageColor { get; init; }
        required public ColorARGB? ForegroundColor { get; init; }
        required public IReadOnlyDictionary<Instrument, double> InstrumentScales { get; init; }
    }
}