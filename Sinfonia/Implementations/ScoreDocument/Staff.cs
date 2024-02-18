using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Staff : ScoreElement, IMementoElement<StaffMemento>, ILayoutElement<IStaffLayout>
    {
        private IStaffLayout staffLayout;
        private readonly int index;

        public int IndexInStaffGroup => index;

        public Guid Guid { get; }
        public double LineSpacing { get => ReadLayout().LineSpacing; set => ReadLayout().LineSpacing = value; }
        public double DistanceToNext { get => ReadLayout().DistanceToNext; set => ReadLayout().DistanceToNext = value; }



        public Staff(StaffLayout staffLayout, int index, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.staffLayout = staffLayout;
            this.index = index;
            Guid = guid;
        }



        public IStaffLayout ReadLayout()
        {
            return staffLayout;
        }
        public void ApplyLayout(IStaffLayout memento)
        {
            staffLayout = memento;
        }
        public StaffMemento GetMemento()
        {
            return new StaffMemento()
            {
                Layout = ReadLayout().Copy(),
                IndexInStaffGroup = IndexInStaffGroup,
                Guid = Guid
            };
        }
        public void ApplyMemento(StaffMemento memento)
        {
            ApplyLayout(memento.Layout);
        }
        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            yield break;
        }
    }
}
