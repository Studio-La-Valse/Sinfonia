namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class PageReaderProxy : IPageReader
    {
        private readonly Page page;

        public PageReaderProxy(Page page)
        {
            this.page = page;
        }

        public Guid Guid => page.Guid;

        public int IndexInScore => page.IndexInScore;

        public int Id => page.IndexInScore;

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return EnumerateStaffSystems();
        }

        public IEnumerable<IStaffSystemReader> EnumerateStaffSystems()
        {
            return page.StaffSystems.Where(s => s.ScoreMeasures.Count > 0).Select(s => s.Proxy());
        }
    }
}
