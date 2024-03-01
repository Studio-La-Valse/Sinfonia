using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class Staff : ScoreElement, IStaff, IUniqueScoreElement
    {
        public int IndexInStaffGroup { get; }



        public Staff(int indexInStaffGroup, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            IndexInStaffGroup = indexInStaffGroup;
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            yield break;
        }
    }
}
