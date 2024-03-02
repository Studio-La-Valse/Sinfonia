namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Staff : ScoreElement
    {
        public int IndexInStaffGroup { get; }



        public Staff(int indexInStaffGroup, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            IndexInStaffGroup = indexInStaffGroup;
        }
    }
}
