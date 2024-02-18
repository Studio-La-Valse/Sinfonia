namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// Represents the layout of a score element.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IScoreElementLayout<T> where T : IScoreElementLayout<T>
    {
        /// <summary>
        /// Deep copy the layout to a new instance.
        /// </summary>
        /// <returns></returns>
        T Copy();
    }
}
