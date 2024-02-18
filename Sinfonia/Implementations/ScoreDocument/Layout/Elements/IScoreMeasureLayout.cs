namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a score measure.
    /// </summary>
    public interface IScoreMeasureLayout : IScoreElementLayout<IScoreMeasureLayout>
    {
        /// <summary>
        /// The key signature of the score measure.
        /// </summary>
        KeySignature KeySignature { get; set; }
        /// <summary>
        /// Specifies how much space is used at the beginning of the measure before content is drawn.
        /// </summary>
        double PaddingLeft { get; set; }
        /// <summary>
        /// Spcifies how much space is reserved at the end of the measure.
        /// </summary>
        double PaddingRight { get; set; }
        /// <summary>
        /// The relative width of the measure. 
        /// </summary>
        double Width { get; set; }
        /// <summary>
        /// Specifies whether the score measure starts a new <see cref="IStaffSystem"/>.
        /// </summary>
        bool IsNewSystem { get; set; }
    }
}