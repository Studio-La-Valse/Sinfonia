namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffSystem : ScoreElement
    {
        private readonly IEnumerable<ScoreMeasure> scoreMeasures;
        private readonly ScoreDocumentCore scoreDocument;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<Guid, StaffGroup> staffGroups = [];

        public StaffSystem(IEnumerable<ScoreMeasure> scoreMeasures, ScoreDocumentCore scoreDocument, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.scoreMeasures = scoreMeasures;
            this.scoreDocument = scoreDocument;
            this.keyGenerator = keyGenerator;
        }


        public IEnumerable<ScoreMeasure> EnumerateMeasures()
        {
            return scoreMeasures;
        }

        public IEnumerable<StaffGroup> EnumerateStaffGroups()
        {
            foreach (var instrumentRibbon in scoreDocument.EnumerateRibbonsCore())
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
    }
}
