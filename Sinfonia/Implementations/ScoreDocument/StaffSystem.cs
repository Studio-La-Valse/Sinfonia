using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Primitives;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class StaffSystem 
    {
        private readonly ScoreDocumentCore scoreDocument;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;

        public IList<ScoreMeasure> ScoreMeasures { get; } = [];
        public StaffSystemLayout Layout
        {
            get
            {
                var paddingBottom = ScoreMeasures.Max(m => m.Layout._PaddingBottom.Field);
                paddingBottom ??= documentStyleTemplate.StaffSystemStyleTemplate.PaddingBottom;
                return new StaffSystemLayout(paddingBottom.Value);
            }
        }


        public StaffSystem(ScoreDocumentCore scoreDocument, ScoreDocumentStyleTemplate documentStyleTemplate)
        {
            this.scoreDocument = scoreDocument;
            this.documentStyleTemplate = documentStyleTemplate;
        }


        public IEnumerable<ScoreMeasure> EnumerateMeasures()
        {
            return ScoreMeasures;
        }

        public IEnumerable<StaffGroup> EnumerateStaffGroups()
        {
            return scoreDocument.EnumerateRibbonsCore().Select(r => new StaffGroup(r, documentStyleTemplate, ScoreMeasures));
        }
    }
}
