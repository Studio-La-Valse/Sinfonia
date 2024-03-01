using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class StaffSystemGenerator
    {
        private readonly IKeyGenerator<int> keyGenerator;
        private IList<StaffSystem> staffSystems = [];

        public StaffSystemGenerator(IKeyGenerator<int> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }

        IStaffSystem GetOrThrow(int index, IScoreDocumentReader scoreDocument, IEnumerable<IScoreMeasureReader> scoreMeasures)
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

        public IEnumerable<IStaffSystem> EnumerateStaffSystems(IScoreDocumentReader scoreDocument, ScoreDocumentLayout scoreLayout)
        {
            var measuresForSystem = new List<IScoreMeasureReader>();
            var staffIndex = 0;

            foreach (var measure in scoreDocument.ReadScoreMeasures())
            {
                measuresForSystem.Add(measure);

                if (scoreLayout.BreakSystem(measuresForSystem))
                {
                    var system = GetOrThrow(staffIndex, scoreDocument, measuresForSystem);
                    yield return system;
                    measuresForSystem = new List<IScoreMeasureReader>();
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
