using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class StaffLayout : IStaffLayout
    {
        public double DistanceToNext { get; }



        public StaffLayout(double distanceToNext)
        {
            DistanceToNext = distanceToNext;
        }
    }
}
