using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffSystem : ScoreElement, IMementoElement<StaffSystemMemento>
    {
        private readonly ScoreMeasure firstMeasure;
        private readonly Guid guid;
        private readonly Dictionary<InstrumentRibbon, StaffGroup> staffGroups = [];
        private readonly IKeyGenerator<int> keyGenerator;
        private IStaffSystemLayout layout;


        public Guid Guid => guid;
        public double PaddingTop { get => ReadLayout().PaddingTop; set => ReadLayout().PaddingTop = value; }


        public StaffSystem(IKeyGenerator<int> keyGenerator, StaffSystemLayout layout, ScoreMeasure firstMeasure, Guid guid) : base(keyGenerator)
        {
            this.keyGenerator = keyGenerator;
            this.layout = layout;
            this.firstMeasure = firstMeasure;
            this.guid = guid;
        }



        public IEnumerable<StaffGroup> EnumerateStaffGroupsCore()
        {
            return staffGroups.Select(e => e.Value);
        }




        public StaffGroup GetStaffGroupCore(int indexInScore)
        {
            return staffGroups.First(s => s.Key.IndexInScore == indexInScore).Value;
        }



        public IEnumerable<ScoreMeasure> ReadMeasures()
        {
            var measure = firstMeasure;
            yield return measure;

            while (true)
            {
                if (measure.TryReadNext(out measure))
                {
                    if (measure.IsNewSystem)
                    {
                        yield break;
                    }

                    yield return measure;
                    continue;
                }

                yield break;
            }
        }


        public IStaffSystemLayout ReadLayout()
        {
            return layout;
        }
        public void ApplyLayout(IStaffSystemLayout layout)
        {
            this.layout = layout;
        }


        public void Register(InstrumentRibbon instrumentRibbon)
        {
            var layout = new StaffGroupLayout(instrumentRibbon.Instrument);
            var staffGroup = new StaffGroup(layout, instrumentRibbon, this, keyGenerator, Guid.NewGuid());
            staffGroups.TryAdd(instrumentRibbon, staffGroup);
        }
        public void Unregister(InstrumentRibbon instrumentRibbon)
        {
            staffGroups.Remove(instrumentRibbon);
        }

        public StaffSystemMemento GetMemento()
        {
            return new StaffSystemMemento()
            {
                Layout = ReadLayout().Copy(),
                StaffGroups = EnumerateStaffGroupsCore().Select(g => g.GetMemento()).ToArray(),
                Guid = Guid
            };
        }
        public void ApplyMemento(StaffSystemMemento memento)
        {
            ApplyLayout(memento.Layout);
            foreach (var staffGroupMemento in memento.StaffGroups)
            {
                var staffGroup = GetStaffGroupCore(staffGroupMemento.IndexInScore);
                staffGroup.ApplyMemento(staffGroupMemento);
            }
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return EnumerateStaffGroupsCore();
        }
    }
}
