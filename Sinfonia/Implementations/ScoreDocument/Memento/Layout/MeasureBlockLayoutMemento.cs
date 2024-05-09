namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class MeasureBlockLayoutMemento
    {
        public required Guid Id { get; init; }
        public required double? StemLength { get; init; }
        public required double? BeamAngle { get; init; }
    }
}
