using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class StaffGroupProxy : IStaffGroupReader
    {
        private readonly StaffGroup staffGroup;



        public IInstrumentRibbonReader InstrumentRibbon => staffGroup.InstrumentRibbon.ProxyReader();

        public Instrument Instrument => staffGroup.Instrument;

        public int IndexInSystem => staffGroup.IndexInSystem;




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
            return staffGroup.EnumerateMeasures().Select(e => e.ProxyReader());
        }

        public IEnumerable<IStaffReader> EnumerateStaves(int numberOfStaves)
        {
            return staffGroup.EnumerateStaves(numberOfStaves).Select(e => e.Proxy());
        }

        public IStaffGroupLayout ReadLayout()
        {
            return staffGroup.Layout;
        }
    }
}
