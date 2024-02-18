namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    /// <summary>
    /// Represents the data necessary to create a voice in a <see cref="IInstrumentMeasureEditor"/>.
    /// </summary>
    public class RibbonMeasureVoiceMemento
    {
        /// <summary>
        /// The voice of the ribbon measure voice group.
        /// </summary>
        public required int Voice { get; init; }
        /// <summary>
        /// The chord groups in the voice group.
        /// </summary>
        public required IEnumerable<MeasureBlockMemento> MeasureBlocks { get; init; }
    }
}
