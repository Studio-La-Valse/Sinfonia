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
            return source.EnumerateMeasuresCore().Select(e => e.Proxy());
        }

        public IEnumerable<IInstrumentRibbonReader> ReadInstrumentRibbons()
        {
            return source.EnumerateRibbonsCore().Select(e => e.Proxy());
        }

        public IInstrumentRibbonReader ReadInstrumentRibbon(int indexInScore)
        {
            return source.GetInstrumentRibbonCore(indexInScore).Proxy();
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (IScoreMeasureReader ribbon in ReadScoreMeasures())
            {
                yield return ribbon;
            }

            foreach (IInstrumentRibbonReader measure in ReadInstrumentRibbons())
            {
                yield return measure;
            }

            foreach (var page in EnumeratePages())
            {
                yield return page;
            }
        }

        public IScoreMeasureReader ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).Proxy();
        }

        public IEnumerable<IPageReader> EnumeratePages()
        {
            return source.GeneratePages().Select(p => p.Proxy());
        }
    }
}
