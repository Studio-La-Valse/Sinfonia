using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class Staff
    {
        private readonly StaffStyleTemplate staffStyleTemplate;
        private readonly IEnumerable<InstrumentMeasure> measures;

        public int IndexInStaffGroup { get; }
        public StaffLayout Layout
        {
            get
            {
                var paddingBottom = measures.Max(m => m.Layout.GetPaddingBottom(IndexInStaffGroup));
                paddingBottom ??= staffStyleTemplate.DistanceToNext;
                var layout = new StaffLayout(paddingBottom.Value);
                return layout;
            }
        }

        public Staff(int indexInStaffGroup, StaffStyleTemplate staffStyleTemplate, IEnumerable<InstrumentMeasure> measures)
        {
            this.staffStyleTemplate = staffStyleTemplate;
            this.measures = measures;

            IndexInStaffGroup = indexInStaffGroup;
        }
    }
}
