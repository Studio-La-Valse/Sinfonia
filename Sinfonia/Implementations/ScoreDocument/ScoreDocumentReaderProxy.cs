using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Reader.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class ScoreDocumentReaderProxy : IScoreDocumentReader
    {
        private readonly ScoreDocumentCore source;

        public int NumberOfMeasures => source.NumberOfMeasures;

        public int NumberOfInstruments => source.NumberOfInstruments;

        public int Id => source.Id;



        public ScoreDocumentReaderProxy(ScoreDocumentCore score)
        {
            source = score;
        }







        public IEnumerable<IScoreMeasureReader> ReadScoreMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyReader());
        }

        public IEnumerable<IInstrumentRibbonReader> ReadInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.ProxyReader());
        }

        public IInstrumentRibbonReader ReadInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).ProxyReader();
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (var ribbon in ReadScoreMeasures())
            {
                yield return ribbon;
            }

            foreach (var measure in ReadInstrumentRibbons())
            {
                yield return measure;
            }

            foreach(var page in this.ReadPages())
            {
                yield return page;
            }
        }

        public IScoreMeasureReader ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).ProxyReader();
        }

        public override string ToString()
        {
            return $"Score Document";
        }

        public IScoreDocumentLayout ReadLayout()
        {
            return source.AuthorLayout;
        }
    }
}
