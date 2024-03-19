namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Page : ScoreElement
    {
        private readonly ScoreDocumentCore scoreDocument;

        public IList<StaffSystem> StaffSystems { get; } = [];
        public int IndexInScore { get; }


        public ScoreDocumentCore HostScoreDocument => scoreDocument;


        public Page(int indexInScore, ScoreDocumentCore scoreDocument, int id, Guid guid) : base(id, guid)
        {
            IndexInScore = indexInScore;
            this.scoreDocument = scoreDocument;
        }
    }
}

