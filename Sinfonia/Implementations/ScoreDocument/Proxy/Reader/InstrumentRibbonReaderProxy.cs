using StudioLaValse.ScoreDocument.Primitives;
using StudioLaValse.ScoreDocument.Reader;

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
            return source.GetMeasureCore(measureIndex).ProxyReader();
        }

        public IEnumerable<IInstrumentMeasureReader> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyReader());
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }
        public override string ToString()
        {
            return $"Instrument Ribbon : [{Guid}]";
        }

        public IInstrumentRibbonLayout ReadLayout()
        {
            return source.Layout;
        }
    }
}
