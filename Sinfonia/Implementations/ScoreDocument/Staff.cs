namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Staff : ScoreElement
    {
        public int IndexInStaffGroup { get; }



        public Staff(int indexInStaffGroup, int id, Guid guid) : base(id, guid)
        {
            IndexInStaffGroup = indexInStaffGroup;
        }
    }
}
