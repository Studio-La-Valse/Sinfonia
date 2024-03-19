namespace Sinfonia.Implementations.ScoreDocument
{
    internal class Staff : ScoreElement
    {
        private readonly ScoreDocumentCore scoreDocument;

        public int IndexInStaffGroup { get; }

        public ScoreDocumentCore HostScoreDocument => scoreDocument;

        public Staff(int indexInStaffGroup, ScoreDocumentCore scoreDocument, int id, Guid guid) : base(id, guid)
        {
            IndexInStaffGroup = indexInStaffGroup;
            this.scoreDocument = scoreDocument;
        }
    }
}
