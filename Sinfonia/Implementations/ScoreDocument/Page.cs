namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Page : ScoreElement
    {
        public IList<StaffSystem> StaffSystems { get; } = [];
        public int IndexInScore { get; }


        public Page(int indexInScore, int id, Guid guid) : base(id, guid)
        {
            IndexInScore = indexInScore;
        }
    }
}

