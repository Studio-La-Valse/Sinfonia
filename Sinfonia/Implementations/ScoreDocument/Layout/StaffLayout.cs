using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class StaffLayout : IStaffLayout
    {
        private readonly ValueTemplateProperty<double> distanceToNext;
        private readonly StaffStyleTemplate styleTemplate;

        public double DistanceToNext 
        { 
            get => distanceToNext.Value; 
            set => distanceToNext.Value = value; 
        }



        public StaffLayout(StaffStyleTemplate styleTemplate, double? distanceToNext)
        {
            this.styleTemplate = styleTemplate;

            this.distanceToNext = new ValueTemplateProperty<double>(() => styleTemplate.DistanceToNext)
            {
                Field = distanceToNext
            };
        }
    }
}
