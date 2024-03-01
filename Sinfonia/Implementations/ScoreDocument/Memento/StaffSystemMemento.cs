namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class StaffSystemMemento
    {
        public required IEnumerable<StaffGroupMemento> StaffGroups { get; init; }
        public required Guid Guid { get; init; }
    }
}
