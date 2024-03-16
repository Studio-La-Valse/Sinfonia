using StudioLaValse.Key;

namespace Sinfonia.Native.Scenes
{
    /// <summary>
    /// Represents a persistent, unique score element.
    /// </summary>
    internal abstract class ScoreElement
    {
        /// <inheritdoc/>
        public ScoreElement(IKeyGenerator<int> keyGenerator, Guid guid)
        {
            Id = keyGenerator.Generate();
            Guid = guid;
        }
        /// <inheritdoc/>
        public ScoreElement(int id, Guid guid)
        {
            Id = id;
            Guid = guid;
        }
        /// <inheritdoc/>
        public int Id { get; }

        /// <inheritdoc/>
        public Guid Guid { get; }
    }
}
