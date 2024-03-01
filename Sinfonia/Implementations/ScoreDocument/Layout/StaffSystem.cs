using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    internal class StaffSystem : ScoreElement, IStaffSystem, IUniqueScoreElement
    {
        private readonly IEnumerable<IScoreMeasureReader> scoreMeasures;
        private readonly IScoreDocumentReader scoreDocument;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<Guid, IStaffGroup> staffGroups = [];

        public StaffSystem(IEnumerable<IScoreMeasureReader> scoreMeasures, IScoreDocumentReader scoreDocument, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.scoreMeasures = scoreMeasures;
            this.scoreDocument = scoreDocument;
            this.keyGenerator = keyGenerator;
        }


        public IEnumerable<IScoreMeasureReader> EnumerateMeasures()
        {
            return scoreMeasures;
        }

        public IEnumerable<IStaffGroup> EnumerateStaffGroups()
        {
            foreach (var instrumentRibbon in scoreDocument.ReadInstrumentRibbons())
            {
                if (staffGroups.TryGetValue(instrumentRibbon.Guid, out var group))
                {
                    yield return group;
                    break;
                }

                group = new StaffGroup(instrumentRibbon, scoreMeasures, keyGenerator, Guid.NewGuid());
                staffGroups[instrumentRibbon.Guid] = group;
                yield return group;
            }
        }

        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            foreach (var staffGroup in EnumerateStaffGroups())
            {
                yield return staffGroup;
            }

            foreach (var measure in EnumerateMeasures())
            {
                yield return measure;
            }
        }
    }
}
