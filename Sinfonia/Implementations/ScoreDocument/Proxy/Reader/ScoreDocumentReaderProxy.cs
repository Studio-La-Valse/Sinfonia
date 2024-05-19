using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class ScoreDocumentReaderProxy : IScoreDocumentReader
    {
        private readonly ScoreDocumentCore source;

        public int NumberOfMeasures => source.NumberOfMeasures;
        public int NumberOfInstruments => source.NumberOfInstruments;
        public int Id => source.Id;
        public Guid Guid => source.Guid;



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
        }

        public IScoreMeasureReader ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).ProxyReader();
        }

        public IEnumerable<IPageReader> GeneratePages()
        {
            return source.GeneratePages().Select(e => e.Proxy());
        }


        public override string ToString()
        {
            return $"Score Document : [{Guid}]";
        }

        public IScoreDocumentLayout ReadLayout()
        {
            return source.SecondaryLayout;
        }
    }
}
