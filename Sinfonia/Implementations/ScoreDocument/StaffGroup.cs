namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffGroup : ScoreElement
    {
        private readonly IEnumerable<ScoreMeasure> scoreMeasures;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly IList<(Guid guid, int id)> staves = [];


        public InstrumentRibbon InstrumentRibbon { get; }
        public Instrument Instrument => InstrumentRibbon.Instrument;
        public int IndexInSystem => InstrumentRibbon.IndexInScore;

         
        public StaffGroup(InstrumentRibbon instrumentRibbon, IEnumerable<ScoreMeasure> scoreMeasures, IKeyGenerator<int> keyGenerator, Guid guid, int id, IList<(Guid guid, int id)> staves) : base(id, guid)
        {
            this.scoreMeasures = scoreMeasures;
            this.keyGenerator = keyGenerator;
            this.staves = staves;   

            InstrumentRibbon = instrumentRibbon;
        }


        public IEnumerable<Staff> EnumerateStaves()
        {
            for (int i = 0; i < staves.Count; i++)
            {
                yield return new Staff(i, staves[i].id, staves[i].guid);
            }
        }

        public IEnumerable<Staff> EnumerateStaves(int numberOfStaves)
        {
            for (int i = 0; i < numberOfStaves; i++)
            {
                if (staves.Count > i)
                {
                    var existingStaff = staves[i];
                    yield return new Staff(i, existingStaff.id, existingStaff.guid);
                    continue;
                }

                var (newGuid, newId) = (Guid.NewGuid(), keyGenerator.Generate());
                staves.Add((newGuid, newId));
                yield return new Staff(i, newId, newGuid);
            }
        }

        public IEnumerable<InstrumentMeasure> EnumerateMeasures()
        {
            return scoreMeasures.Select(e => e.GetMeasureCore(InstrumentRibbon.IndexInScore));
        }
    }
}
