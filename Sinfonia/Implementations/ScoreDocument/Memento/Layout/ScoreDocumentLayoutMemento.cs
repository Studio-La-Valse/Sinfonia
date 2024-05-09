using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;

namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class ScoreDocumentLayoutMemento
    {
        required public Guid Id { get; init; }
        public double? Scale { get; init; }
        public double? HorizontalStaffLineThickness { get; init; }
        public double? VerticalStaffLineThickness { get; init; }
        public double? StemLineThickness { get; init; }
        public double? FirstSystemIndent { get; init; }
        public ColorARGB? PageColor { get; init; }
        public ColorARGB? ForegroundColor { get; init; }
        public IReadOnlyDictionary<Guid, double> InstrumentScales { get; init; } = new Dictionary<Guid, double>();

        public static ScoreDocumentLayoutMemento Create()
        {
            return new ScoreDocumentLayoutMemento()
            {
                Id = Guid.NewGuid()
            };
        }
    }
}