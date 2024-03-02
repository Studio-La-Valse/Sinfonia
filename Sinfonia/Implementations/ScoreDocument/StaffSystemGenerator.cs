using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffSystemGenerator
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private IList<StaffSystem> staffSystems = [];

        public StaffSystemGenerator(IKeyGenerator<int> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }

        private StaffSystem GetOrThrow(int index, ScoreDocumentCore scoreDocument, IEnumerable<ScoreMeasure> scoreMeasures)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

            if (index < staffSystems.Count)
            {
                return staffSystems[index];
            }

            if (index == staffSystems.Count)
            {
                staffSystems.Add(new StaffSystem(scoreMeasures, scoreDocument, keyGenerator, Guid.NewGuid()));
                return staffSystems[index];
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        public IEnumerable<StaffSystem> EnumerateStaffSystems(ScoreDocumentCore scoreDocument, ScoreDocumentLayout scoreLayout)
        {
            var measuresForSystem = new List<ScoreMeasure>();
            var staffIndex = 0;

            foreach (var measure in scoreDocument.EnumerateMeasuresCore())
            {
                measuresForSystem.Add(measure);

                if (scoreLayout.BreakSystem(measuresForSystem.Select(e => e.Proxy())))
                {
                    var system = GetOrThrow(staffIndex, scoreDocument, measuresForSystem);
                    yield return system;
                    measuresForSystem = [];
                    staffIndex++;
                }
            }

            if (measuresForSystem.Count != 0)
            {
                var system = GetOrThrow(staffIndex, scoreDocument, measuresForSystem);
                yield return system;
            }
        }
    }
}
