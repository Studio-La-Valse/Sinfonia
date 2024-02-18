namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// Represents a chord layout.
    /// </summary>
    public interface IChordLayout : IScoreElementLayout<IChordLayout>
    {
        /// <summary>
        /// The total x offset for all notes in the chord.
        /// </summary>
        double XOffset { get; set; }
        /// <summary>
        /// A dictionary containing the beams for the chord.
        /// Modifying this dictionary manually is discouraged as it may lead to unwanted behaviour.
        /// The dictionary is filled by the <see cref="IMeasureBlockEditor"/>'s <see cref="IMeasureBlockEditor.Rebeam"/> method.
        /// </summary>
        Dictionary<PowerOfTwo, BeamType> Beams { get; }
    }
}