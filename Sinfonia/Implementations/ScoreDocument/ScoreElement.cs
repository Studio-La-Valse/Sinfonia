namespace Sinfonia.Implementations.ScoreDocument
{
    /// <summary>
    /// Represents a persistent, unique score element.
    /// </summary>
    public abstract class ScoreElement : IUniqueScoreElement
    {
        private readonly int elementId;

        /// <inheritdoc/>
        public ScoreElement(IKeyGenerator<int> keyGenerator, Guid guid)
        {
            elementId = keyGenerator.Generate();
            Guid = guid;
        }

        /// <inheritdoc/>
        public int Id => elementId;

        /// <inheritdoc/>
        public Guid Guid { get; }

        /// <inheritdoc/>
        public bool Equals(IUniqueScoreElement? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.Id == Id;
        }

        /// <inheritdoc/>
        public abstract IEnumerable<IUniqueScoreElement> EnumerateChildren();
    }
}
