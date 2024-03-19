namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffSystem : ScoreElement
    {
        private readonly ScoreDocumentCore scoreDocument;
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly Dictionary<Guid, (Guid guid, int id, IList<(Guid guid, int id)> staves)> staffGroups;

        public IList<ScoreMeasure> ScoreMeasures { get; } = [];

        public ScoreDocumentCore HostScoreDocument => scoreDocument;

        public StaffSystem(ScoreDocumentCore scoreDocument, IKeyGenerator<int> keyGenerator, Guid guid, int id, Dictionary<Guid, (Guid guid, int id, IList<(Guid guid, int id)> staves)> staffGroups) : base(id, guid)
        {

            this.scoreDocument = scoreDocument;
            this.keyGenerator = keyGenerator;
            this.staffGroups = staffGroups;
        }


        public IEnumerable<ScoreMeasure> EnumerateMeasures()
        {
            return ScoreMeasures;
        }

        public IEnumerable<StaffGroup> EnumerateStaffGroups()
        {
            foreach (var instrumentRibbon in scoreDocument.EnumerateRibbonsCore())
            {
                if (staffGroups.TryGetValue(instrumentRibbon.Guid, out var groupGenerator))
                {
                    yield return new StaffGroup(instrumentRibbon, ScoreMeasures, keyGenerator, groupGenerator.guid, groupGenerator.id, groupGenerator.staves);
                    continue;
                }

                var (newGuid, newId, newStaves) = (Guid.NewGuid(), keyGenerator.Generate(), new List<(Guid, int)>());
                staffGroups[instrumentRibbon.Guid] = (newGuid, newId, newStaves);
                yield return new StaffGroup(instrumentRibbon, ScoreMeasures, keyGenerator, newGuid, newId, newStaves);
            }
        }
    }
}
