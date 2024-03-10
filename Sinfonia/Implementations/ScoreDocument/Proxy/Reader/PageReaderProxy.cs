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

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return EnumerateStaffSystems();
        }

        public IEnumerable<IStaffSystemReader> EnumerateStaffSystems()
        {
            return page.StaffSystems.Select(s => s.Proxy());
        }
    }
}
