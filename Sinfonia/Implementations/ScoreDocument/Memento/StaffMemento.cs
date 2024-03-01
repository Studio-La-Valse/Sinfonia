namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class StaffMemento
    {
        public required int IndexInStaffGroup { get; init; }
        public required Guid Guid { get; init; }
    }
}
