using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class StaffGroupMemento
    {
        public required int IndexInScore { get; init; }
        public required IStaffGroupLayout Layout { get; init; }
        public required IEnumerable<StaffMemento> Staves { get; init; }
        public required Guid Guid { get; init; }
    }
}
