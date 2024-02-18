namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of a note.
    /// </summary>
    public interface INoteLayout : IScoreElementLayout<INoteLayout>
    {
        /// <summary>
        /// Forces a accental before the note.
        /// </summary>
        AccidentalDisplay ForceAccidental { get; set; }
        /// <summary>
        /// The staff index of the note.
        /// </summary>
        int StaffIndex { get; set; }
        /// <summary>
        /// The individual x offset of the note.
        /// This values is accumulated with the <see cref="IChordLayout.XOffset"/> values.
        /// </summary>
        double XOffset { get; set; }
    }
}