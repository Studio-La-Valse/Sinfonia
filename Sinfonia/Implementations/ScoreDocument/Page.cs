using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class Page 
    {
        private readonly ScoreDocumentCore scoreDocument;

        public IList<StaffSystem> StaffSystems { get; } = [];
        public IPageLayout Layout { get; }
        public int IndexInScore { get; }


        public ScoreDocumentCore HostScoreDocument => 
            scoreDocument;


        public Page(int indexInScore, ScoreDocumentCore scoreDocument, ScoreDocumentStyleTemplate styleTemplate) 
        {
            this.scoreDocument = scoreDocument;

            IndexInScore = indexInScore;

            Layout = new PageLayout(styleTemplate.PageStyleTemplate);
        }
    }
}

