namespace Sinfonia.Implementations.ScoreDocument
{
    public abstract class ScoreElement : IUniqueScoreElement
    {
        public int Id { get; }
        public Guid Guid { get; }


        public ScoreElement(IKeyGenerator<int> keyGenerator, Guid guid)
        {
            Id = keyGenerator.Generate();
            Guid = guid;
        }
        public ScoreElement(int id, Guid guid)
        {
            Id = id;
            Guid = guid;
        }
    }
}
