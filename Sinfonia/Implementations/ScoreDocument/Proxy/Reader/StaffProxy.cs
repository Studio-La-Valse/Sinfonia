using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffReaderProxy : IStaffReader
    {
        private readonly Staff staff;

        public int IndexInStaffGroup => staff.IndexInStaffGroup;


        public StaffReaderProxy(Staff staff)
        {
            this.staff = staff;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            yield break;
        }

        public IStaffLayout ReadLayout()
        {
            return staff.Layout;
        }
    }
}
