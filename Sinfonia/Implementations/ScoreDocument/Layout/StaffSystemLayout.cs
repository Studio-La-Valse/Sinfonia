using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class StaffSystemLayout : IStaffSystemLayout
    {
        private readonly StaffSystemStyleTemplate styleTemplate;
        private readonly ValueTemplateProperty<double> paddingBottom;

        public double PaddingBottom { get => paddingBottom.Value; }


        public StaffSystemLayout(StaffSystemStyleTemplate styleTemplate, double? paddingBottom)
        {
            this.styleTemplate = styleTemplate;
            this.paddingBottom = new ValueTemplateProperty<double>(() => styleTemplate.PaddingBottom);
            this.paddingBottom.Field = paddingBottom;
        }


        public StaffSystemLayout Copy()
        {
            var copy = new StaffSystemLayout(styleTemplate, paddingBottom.Field);
            return copy;
        }
    }
}
