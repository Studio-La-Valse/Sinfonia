namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a note group.
    /// </summary>
    public class MeasureBlockLayout : IMeasureBlockLayout
    {
        /// <inheritdoc/>
        public double StemLength { get; set; }
        /// <inheritdoc/>
        public double BeamAngle { get; set; }

        /// <summary>
        /// Create the default layout.
        /// </summary>
        public MeasureBlockLayout()
        {
            StemLength = 4;
            BeamAngle = 0;
        }

        /// <summary>
        /// Create a new layout.
        /// </summary>
        /// <param name="stemLength"></param>
        /// <param name="beamAngle"></param>
        public MeasureBlockLayout(double stemLength, double beamAngle)
        {
            StemLength = stemLength;
            BeamAngle = beamAngle;
        }

        /// <inheritdoc/>
        public IMeasureBlockLayout Copy()
        {
            return new MeasureBlockLayout(StemLength, BeamAngle);
        }
    }
}
