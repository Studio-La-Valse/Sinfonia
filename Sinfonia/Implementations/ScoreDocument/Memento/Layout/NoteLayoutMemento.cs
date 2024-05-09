namespace Sinfonia.Implementations.ScoreDocument.Memento.Layout
{
    public class NoteLayoutMemento
    {
        public required Guid Id { get; init; }
        public required int? StaffIndex { get; init; }
        public required double? XOffset { get; init; }
        public required double? Scale { get; init; }
        public required AccidentalDisplay? ForceAccidental { get; init; }
    }
}