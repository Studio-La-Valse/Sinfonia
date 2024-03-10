namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffSystemReaderProxy : IStaffSystemReader
    {
        private readonly StaffSystem staffSystem;



        public int Id => staffSystem.Id;

        public Guid Guid => staffSystem.Guid;



        public StaffSystemReaderProxy(StaffSystem staffSystem)
        {
            this.staffSystem = staffSystem;
        }



        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (IScoreMeasureReader scoreMeasure in EnumerateMeasures())
            {
                yield return scoreMeasure;
            }

            foreach (IStaffGroupReader staffGroup in EnumerateStaffGroups())
            {
                yield return staffGroup;
            }
        }

        public IEnumerable<IScoreMeasureReader> EnumerateMeasures()
        {
            return staffSystem.EnumerateMeasures().Select(e => e.Proxy());
        }

        public IEnumerable<IStaffGroupReader> EnumerateStaffGroups()
        {
            return staffSystem.EnumerateStaffGroups().Select(e => e.Proxy());
        }
    }
}
