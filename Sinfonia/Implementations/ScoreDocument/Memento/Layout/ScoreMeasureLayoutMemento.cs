namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class ScoreMeasureLayoutMemento
    {
        public required Guid Id { get; init; }
        public required KeySignature? KeySignature { get; init; }
        public required double? PaddingLeft { get; init; }
        public required double? PaddingRight { get; init; }
        public required double? PaddingBottom { get; init; }
        public required double? Width { get; init; }
    }
}