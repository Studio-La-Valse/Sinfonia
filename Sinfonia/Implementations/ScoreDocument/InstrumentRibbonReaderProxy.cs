using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentRibbonReaderProxy : IInstrumentRibbonReader
    {
        private readonly InstrumentRibbon source;




        public int Id => source.Id;

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
            return $"Instrument Ribbon : [{Instrument}]";
        }

        public IInstrumentRibbonLayout ReadLayout()
        {
            return source.AuthorLayout;
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            if (other is null)
            {
                return false;
            }

            return other.Id == Id;
        }
    }
}
