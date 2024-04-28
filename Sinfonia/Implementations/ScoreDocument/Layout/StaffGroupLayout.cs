using StudioLaValse.ScoreDocument.Core;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    /// <summary>
    /// The layout of a staff group.
    /// </summary>
    public class StaffGroupLayout : IStaffGroupLayout
    {
        private readonly StaffGroupStyleTemplate styleTemplate;
        private readonly Instrument instrument;
        private readonly ValueTemplateProperty<int> numberOfStaves;
        private readonly ValueTemplateProperty<double> distanceToNext;

        public bool Collapsed { get; set; }


        public int NumberOfStaves 
        { 
            get => numberOfStaves.Value; 
            set => numberOfStaves.Value = value; 
        }
        public double DistanceToNext 
        { 
            get => distanceToNext.Value; 
            set => distanceToNext.Value = value; 
        }


        public StaffGroupLayout(StaffGroupStyleTemplate styleTemplate, Instrument instrument, int? numberOfStaves, double? distanceToNext, bool collapsed)
        {
            this.styleTemplate = styleTemplate;
            this.instrument = instrument;

            this.numberOfStaves = new ValueTemplateProperty<int>(() => instrument.NumberOfStaves);
            this.numberOfStaves.Field = numberOfStaves;
            this.distanceToNext = new ValueTemplateProperty<double>(() => styleTemplate.DistanceToNext);
            this.distanceToNext.Field = distanceToNext;

            Collapsed = collapsed;
        }
    }
}
