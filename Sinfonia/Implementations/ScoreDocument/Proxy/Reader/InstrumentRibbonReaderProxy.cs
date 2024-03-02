namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class InstrumentRibbonReaderProxy : IInstrumentRibbonReader
    {
        private readonly InstrumentRibbon source;




        public int Id => source.Id;

        public Guid Guid => source.Guid;

        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;




        public InstrumentRibbonReaderProxy(InstrumentRibbon source)
        {
            this.source = source;
        }



        public IInstrumentMeasureReader ReadMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).Proxy();
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.Proxy());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }
    }
}
