using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    public class StaffMemento
    {
        public required int IndexInStaffGroup { get; init; }
        public required IStaffLayout Layout { get; init; }
        public required Guid Guid { get; init; }
    }
}
