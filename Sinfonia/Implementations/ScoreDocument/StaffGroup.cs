using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class StaffGroup
    {
        private readonly ScoreDocumentStyleTemplate documentStyleTemplate;
        private readonly IList<ScoreMeasure> scoreMeasures;


        public InstrumentRibbon InstrumentRibbon { get; }
        public StaffGroupLayout Layout
        {
            get
            {
                var numberOfStaves = EnumerateMeasures().Max(m => m.Layout.NumberOfStaves);
                var distanceToNext = EnumerateMeasures().Max(m => m.Layout.PaddingBottom);
                var collapsed = EnumerateMeasures().Any(m => m.Layout.Collapsed);

                var layout = new StaffGroupLayout(documentStyleTemplate.StaffGroupStyleTemplate, Instrument, numberOfStaves, distanceToNext, collapsed);
                return layout;
            }
        }


        public Instrument Instrument => 
            InstrumentRibbon.Instrument;
        public int IndexInSystem => 
            InstrumentRibbon.IndexInScore;
        public ScoreDocumentCore HostScoreDocument => 
            InstrumentRibbon.HostScoreDocument;


        public StaffGroup(InstrumentRibbon instrumentRibbon, ScoreDocumentStyleTemplate documentStyleTemplate, IList<ScoreMeasure> scoreMeasures)
        {
            InstrumentRibbon = instrumentRibbon;

            this.scoreMeasures = scoreMeasures;
            this.documentStyleTemplate = documentStyleTemplate;
        }


        public IEnumerable<Staff> EnumerateStaves()
        {
            var numberOfStaves = Layout.NumberOfStaves;

            return EnumerateStaves(numberOfStaves);
        }

        public IEnumerable<Staff> EnumerateStaves(int numberOfStaves)
        {
            for (var staffIndex = 0; staffIndex < numberOfStaves; staffIndex++)
            {
                yield return new Staff(staffIndex, documentStyleTemplate.StaffStyleTemplate, EnumerateMeasures());
            }
        }

        public IEnumerable<InstrumentMeasure> EnumerateMeasures()
        {
            return scoreMeasures.Select(e => e.GetMeasureCore(InstrumentRibbon.IndexInScore));
        }
    }
}
