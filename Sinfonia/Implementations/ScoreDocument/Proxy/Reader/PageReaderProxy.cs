using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class PageReaderProxy : IPageReader
    {
        private readonly Page page;


        public int IndexInScore => page.IndexInScore;



        public PageReaderProxy(Page page)
        {
            this.page = page;
        }



        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return EnumerateStaffSystems();
        }

        public IEnumerable<IStaffSystemReader> EnumerateStaffSystems()
        {
            return page.StaffSystems.Where(s => s.ScoreMeasures.Count > 0).Select(s => s.Proxy());
        }

        public IPageLayout ReadLayout()
        {
            return page.Layout;
        }
    }
}
