namespace Sinfonia.Implementations.ScoreDocument.Memento
{
    /// <summary>
    /// Represents the data necessary to create a <see cref="IScoreDocumentEditor"/>.
    /// </summary>
    public class ScoreDocumentMemento
    {
        /// <summary>
        /// The instrument ribbons in the score document.
        /// </summary>
        public required IEnumerable<InstrumentRibbonMemento> InstrumentRibbons { get; init; }
        /// <summary>
        /// The score measures of the score document.
        /// </summary>
        public required IEnumerable<ScoreMeasureMemento> ScoreMeasures { get; init; }
    }
}
