using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class StaffSystemMemento
    {
        public required IStaffSystemLayout Layout { get; init; }
        public required IEnumerable<StaffGroupMemento> StaffGroups { get; init; }
        public required Guid Guid { get; init; }
    }
}
