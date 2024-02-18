namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <inheritdoc/>
    internal class ChordLayout : IChordLayout
    {
        /// <inheritdoc/>
        public double XOffset { get; set; }
        /// <inheritdoc/>
        public Dictionary<PowerOfTwo, BeamType> Beams { get; }

        /// <summary>
        /// Create a default layout.
        /// </summary>
        /// <param name="xOffset"></param>
        /// <param name="stemLength"></param>
        public ChordLayout(double xOffset = 0)
        {
            XOffset = xOffset;
            Beams = new Dictionary<PowerOfTwo, BeamType>(comparer: new PowerOfTwoEqualityComparer());
        }
        private ChordLayout(Dictionary<PowerOfTwo, BeamType> beams, double xOffset = 0)
        {
            XOffset = xOffset;
            Beams = beams;
        }

        /// <inheritdoc/>
        public IChordLayout Copy()
        {
            var beams = new Dictionary<PowerOfTwo, BeamType>();
            foreach (var entry in Beams)
            {
                beams.Add(entry.Key, entry.Value);
            }

            return new ChordLayout(beams, XOffset);
        }
    }
}