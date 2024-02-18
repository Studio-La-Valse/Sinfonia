namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// A staff layout.
    /// </summary>
    public class StaffLayout : IStaffLayout
    {
        /// <inheritdoc/>
        public double DistanceToNext { get; set; }
        /// <inheritdoc/>
        public double LineSpacing { get; set; }

        /// <summary>
        /// Create a default staff layout.
        /// </summary>
        /// <param name="distanceToNext"></param>
        /// <param name="lineSpacing"></param>
        public StaffLayout(double distanceToNext = 13, double lineSpacing = 1.2)
        {
            DistanceToNext = distanceToNext;
            LineSpacing = lineSpacing;
        }

        /// <inheritdoc/>
        public IStaffLayout Copy()
        {
            return new StaffLayout(DistanceToNext, LineSpacing);
        }
    }
}
