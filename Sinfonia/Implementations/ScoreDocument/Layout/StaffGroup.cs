using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class StaffGroup : ScoreElement, IStaffGroup, IUniqueScoreElement
    {
        private readonly IInstrumentRibbonReader instrumentRibbon;
        private readonly IEnumerable<IScoreMeasureReader> scoreMeasures;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<int, IStaff> staves = [];


        public IInstrumentRibbonReader InstrumentRibbon
        {
            get
            {
                return instrumentRibbon;
            }

        }
        public Instrument Instrument => instrumentRibbon.Instrument;

        public int IndexInSystem => instrumentRibbon.IndexInScore;

        public StaffGroup(IInstrumentRibbonReader instrumentRibbon, IEnumerable<IScoreMeasureReader> scoreMeasures, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.instrumentRibbon = instrumentRibbon;
            this.scoreMeasures = scoreMeasures;
            this.keyGenerator = keyGenerator;
        }


        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            foreach (var measure in EnumerateMeasures())
            {
                yield return measure;
            }

            foreach (var staff in staves.Values)
            {
                yield return staff;
            }
        }

        public IEnumerable<IStaff> EnumerateStaves(int numberOfStaves)
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



        public IEnumerable<IInstrumentMeasureReader> EnumerateMeasures()
        {
            return scoreMeasures.Select(e => e.ReadMeasure(instrumentRibbon.IndexInScore));
        }
    }
}
