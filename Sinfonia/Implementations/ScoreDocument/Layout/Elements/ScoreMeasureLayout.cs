namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a score measure.
    /// </summary>
    public class ScoreMeasureLayout : IScoreMeasureLayout
    {
        /// <inheritdoc/>
        public KeySignature KeySignature { get; set; }
        /// <inheritdoc/>
        public double PaddingLeft { get; set; }
        /// <inheritdoc/>
        public double PaddingRight { get; set; }
        /// <inheritdoc/>
        public double Width { get; set; }
        /// <inheritdoc/>
        public bool IsNewSystem { get; set; }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="keySignature"></param>
        /// <param name="paddingleft"></param>
        /// <param name="paddingright"></param>
        /// <param name="width"></param>
        /// <param name="isNewSystem"></param>
        public ScoreMeasureLayout(KeySignature? keySignature = null, double paddingleft = 10, double paddingright = 5, double width = 100, bool isNewSystem = false)
        {
            KeySignature = keySignature ?? new KeySignature(Step.C, MajorOrMinor.Major);
            PaddingLeft = paddingleft;
            PaddingRight = paddingright;
            Width = width;
            IsNewSystem = isNewSystem;
        }

        /// <inheritdoc/>
        public IScoreMeasureLayout Copy()
        {
            return new ScoreMeasureLayout(KeySignature, PaddingLeft, PaddingRight, Width, IsNewSystem);
        }
    }
}