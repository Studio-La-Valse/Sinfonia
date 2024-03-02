namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffGroup : ScoreElement
    {
        private readonly InstrumentRibbon instrumentRibbon;
        private readonly IEnumerable<ScoreMeasure> scoreMeasures;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<int, Staff> staves = [];


        public InstrumentRibbon InstrumentRibbon => instrumentRibbon;
        public Instrument Instrument => instrumentRibbon.Instrument;
        public int IndexInSystem => instrumentRibbon.IndexInScore;


        public StaffGroup(InstrumentRibbon instrumentRibbon, IEnumerable<ScoreMeasure> scoreMeasures, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.instrumentRibbon = instrumentRibbon;
            this.scoreMeasures = scoreMeasures;
            this.keyGenerator = keyGenerator;
        }


        public IEnumerable<Staff> EnumerateStaves()
        {
            foreach (var staff in staves.Values)
            {
                yield return staff;
            }
        }

        public IEnumerable<Staff> EnumerateStaves(int numberOfStaves)
        {
            for (int i = 0; i < numberOfStaves; i++)
            {
                if (staves.TryGetValue(i, out var staff))
                {
                    yield return staff;
                    break;
                }

                staff = new Staff(i, keyGenerator, Guid.NewGuid());
                staves[i] = staff;
                yield return staff;
            }
        }

        public IEnumerable<InstrumentMeasure> EnumerateMeasures()
        {
            return scoreMeasures.Select(e => e.GetMeasureCore(instrumentRibbon.IndexInScore));
        }
    }
}
