using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Primitives;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class StaffSystem 
    {
        private readonly ScoreDocumentCore scoreDocument;
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;

        public IList<ScoreMeasure> ScoreMeasures { get; } = [];
        public StaffSystemLayout Layout
        {
            get
            {
                double? paddingBottom = null;
                foreach(var measure in ScoreMeasures)
                {
                    var measurePaddingBottom = measure.Layout.PaddingBottom;
                    if(measurePaddingBottom.HasValue)
                    {
                        if (paddingBottom.HasValue)
                        {
                            paddingBottom = Math.Max(paddingBottom.Value, measurePaddingBottom.Value);
                        }
                        else
                        {
                            paddingBottom = measurePaddingBottom.Value;
                        }
                    }
                }
                return new StaffSystemLayout(documentStyleTemplate.StaffSystemStyleTemplate, paddingBottom);
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
