namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffGroupProxy : IStaffGroupReader
    {
        private readonly StaffGroup staffGroup;



        public IInstrumentRibbonReader InstrumentRibbon => staffGroup.InstrumentRibbon.Proxy();

        public Instrument Instrument => staffGroup.Instrument;

        public int IndexInSystem => staffGroup.IndexInSystem;

        public int Id => staffGroup.Id;

        public Guid Guid => staffGroup.Guid;




        public StaffGroupProxy(StaffGroup staffGroup)
        {
            this.staffGroup = staffGroup;
        }




        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (var measure in EnumerateMeasures())
            {
                yield return measure;
            }

            foreach (var staff in staffGroup.EnumerateStaves())
            {
                yield return staff.Proxy();
            }
        }

        public IEnumerable<IInstrumentMeasureReader> EnumerateMeasures()
        {
            return staffGroup.EnumerateMeasures().Select(e => e.Proxy());
        }

        public IEnumerable<IStaffReader> EnumerateStaves(int numberOfStaves)
        {
            return staffGroup.EnumerateStaves(numberOfStaves).Select(e => e.Proxy());
        }
    }
}
