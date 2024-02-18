namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// Represents an instrument measure layout.
    /// </summary>
    public interface IInstrumentMeasureLayout : IScoreElementLayout<IInstrumentMeasureLayout>
    {
        /// <summary>
        /// The clef changes of the measure.
        /// </summary>
        IEnumerable<ClefChange> ClefChanges { get; }
        /// <summary>
        /// Adds a clef change.
        /// </summary>
        /// <param name="clefChange"></param>
        void AddClefChange(ClefChange clefChange);
        /// <summary>
        /// Removes a clef change.
        /// </summary>
        /// <param name="clefChange"></param>
        void RemoveClefChange(ClefChange clefChange);
    }
}