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
            foreach (var ribbon in ReadScoreMeasures())
            {
                yield return ribbon;
            }

            foreach (var measure in ReadInstrumentRibbons())
            {
                yield return measure;
            }

            foreach (var system in EnumerateStaffSystems())
            {
                yield return system;
            }
        }

        public IScoreMeasureReader ReadScoreMeasure(int indexInScore)
        {
            return source.GetScoreMeasureCore(indexInScore).Proxy();
        }

        public IEnumerable<IStaffSystemReader> EnumerateStaffSystems()
        {
            return source.EnumerateStaffSystems().Select(e => e.Proxy());
        }
    }
}
