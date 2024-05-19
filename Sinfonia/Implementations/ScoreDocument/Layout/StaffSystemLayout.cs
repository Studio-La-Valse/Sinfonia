using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class StaffSystemLayout : IStaffSystemLayout
    {
        public double PaddingBottom { get; }


        public StaffSystemLayout(double paddingBottom)
        {
            PaddingBottom = paddingBottom;
        }
    }
}
