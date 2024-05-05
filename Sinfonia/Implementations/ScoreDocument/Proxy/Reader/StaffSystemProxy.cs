using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffSystemReaderProxy : IStaffSystemReader
    {
        private readonly StaffSystem staffSystem;




        public StaffSystemReaderProxy(StaffSystem staffSystem)
        {
            this.staffSystem = staffSystem;
        }



        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (var scoreMeasure in EnumerateMeasures())
            {
                yield return scoreMeasure;
            }
        }

        public IEnumerable<IScoreMeasureReader> EnumerateMeasures()
        {
            return staffSystem.EnumerateMeasures().Select(e => e.ProxyReader());
        }

        public IEnumerable<IStaffGroupReader> EnumerateStaffGroups()
        {
            return staffSystem.EnumerateStaffGroups().Select(e => e.Proxy());
        }

        public IStaffSystemLayout ReadLayout()
        {
            return staffSystem.Layout;
        }
    }
}
