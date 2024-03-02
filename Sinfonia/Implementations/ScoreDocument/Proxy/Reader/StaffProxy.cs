namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffReaderProxy : IStaffReader
    {
        private readonly Staff staff;

        public int IndexInStaffGroup => staff.IndexInStaffGroup;

        public int Id => staff.Id;

        public Guid Guid => staff.Guid;


        public StaffReaderProxy(Staff staff)
        {
            this.staff = staff;
        }


        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            yield break;
        }
    }
}
