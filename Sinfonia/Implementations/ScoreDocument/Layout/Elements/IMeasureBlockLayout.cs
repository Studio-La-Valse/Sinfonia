namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a note group.
    /// </summary>
    public interface IMeasureBlockLayout : IScoreElementLayout<IMeasureBlockLayout>
    {
        /// <summary>
        /// The stem length of the first chord in the group.
        /// </summary>
        double StemLength { get; set; }
        /// <summary>
        /// The angle of the beam of the notegroup.
        /// </summary>
        double BeamAngle { get; set; }
    }
}
