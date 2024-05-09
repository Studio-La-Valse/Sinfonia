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
                double? paddingBottom = null;
                foreach (var padding in measures.Select(m => m.Layout.GetPaddingBottom(IndexInStaffGroup)))
                {
                    if (padding.HasValue)
                    {
                        if (paddingBottom.HasValue)
                        {
                            paddingBottom = Math.Max(padding.Value, paddingBottom.Value);
                        }
                        else
                        {
                            paddingBottom = padding.Value;
                        }
                    }
                }
                var layout = new StaffLayout(staffStyleTemplate, paddingBottom);
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
