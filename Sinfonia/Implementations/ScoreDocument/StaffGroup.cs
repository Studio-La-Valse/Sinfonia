using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffGroup : ScoreElement, IMementoElement<StaffGroupMemento>, ILayoutElement<IStaffGroupLayout>
    {
        private readonly Dictionary<int, Staff> staves = [];
        private readonly InstrumentRibbon host;
        private readonly StaffSystem staffSystem;
        private readonly IKeyGenerator<int> keyGenerator;
        private IStaffGroupLayout layout;

        public Instrument Instrument =>
            host.Instrument;
        public int IndexInScore =>
            host.IndexInScore;

        public Guid Guid { get; }
        public double DistanceToNext { get => ReadLayout().DistanceToNext; set => ReadLayout().DistanceToNext = value; }
        public int NumberOfStaves { get => ReadLayout().NumberOfStaves; set => ReadLayout().NumberOfStaves = value; }
        public bool Collapsed { get => ReadLayout().Collapsed; set => ReadLayout().Collapsed = value; }



        public StaffGroup(StaffGroupLayout layout, InstrumentRibbon host, StaffSystem staffSystem, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.layout = layout;
            this.host = host;
            this.staffSystem = staffSystem;
            this.keyGenerator = keyGenerator;

            Guid = guid;
        }




        public IEnumerable<Staff> EnumerateStavesCore()
        {
            for (var i = 0; i < layout.NumberOfStaves; i++)
            {
                if (!staves.TryGetValue(i, out var staff))
                {
                    var layout = new StaffLayout();
                    staff = new Staff(layout, i, keyGenerator, Guid.NewGuid());
                    staves.Add(i, staff);
                }

                yield return staff;
            }
        }
        public IEnumerable<InstrumentMeasure> ReadMeasures()
        {
            return staffSystem.ReadMeasures().Select(m => m.GetMeasureCore(host.IndexInScore));
        }
        public InstrumentRibbon ReadContext()
        {
            return host;
        }
        public Staff GetStaffCore(int staffIndex)
        {
            if (!staves.TryGetValue(staffIndex, out var staff))
            {
                var layout = new StaffLayout();
                staff = new Staff(layout, staffIndex, keyGenerator, Guid.NewGuid());
                staves.Add(staffIndex, staff);
            }

            return staff;
        }




        public StaffGroupMemento GetMemento()
        {
            return new StaffGroupMemento()
            {
                IndexInScore = IndexInScore,
                Layout = ReadLayout().Copy(),
                Staves = EnumerateStavesCore().Select(e => e.GetMemento()).ToArray(),
                Guid = Guid
            };
        }
        public void ApplyMemento(StaffGroupMemento memento)
        {
            ApplyLayout(memento.Layout);
            foreach (var staffMemento in memento.Staves)
            {
                var staff = GetStaffCore(staffMemento.IndexInStaffGroup);
                staff.ApplyMemento(staffMemento);
            }
        }


        public IStaffGroupLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IStaffGroupLayout memento)
        {
            layout = memento;
        }

        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return EnumerateStavesCore();
        }
    }
}
