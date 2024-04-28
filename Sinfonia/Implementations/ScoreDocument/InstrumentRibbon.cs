using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonMemento>
    {
        private readonly ScoreDocumentCore score;

        public Instrument Instrument { get; }
        public InstrumentRibbonLayout Layout { get; }


        public int IndexInScore => score.IndexOf(this);
        public ScoreDocumentCore HostScoreDocument => score;


        public InstrumentRibbon(ScoreDocumentCore score, Instrument instrument, ScoreDocumentStyleTemplate styleTemplate, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            Instrument = instrument;
            Layout = new(this);
        }


        public InstrumentMeasure GetMeasureCore(int index)
        {
            return score.GetMeasureCore(index, IndexInScore);
        }


        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            return score.EnumerateScoreMeasuresCore(this);
        }




        public InstrumentRibbonMemento GetMemento()
        {
            return new InstrumentRibbonMemento
            {
                Instrument = Instrument,
                Guid = Guid,
                IndexInScore = IndexInScore,
                InstrumentMeasures = EnumerateMeasuresCore().Select(m => m.GetMemento()).ToArray(),
            };
        }
        public void ApplyMemento(InstrumentRibbonMemento memento)
        {
            foreach (var measureMemento in memento.InstrumentMeasures)
            {
                var measure = GetMeasureCore(measureMemento.MeasureIndex);
                measure.ApplyMemento(measureMemento);
            }
        }
    }
}
