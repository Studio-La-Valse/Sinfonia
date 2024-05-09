using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonMemento>
    {
        private readonly ScoreDocumentCore score;

        public Instrument Instrument { get; }
        public InstrumentRibbonLayout Layout { get; }


        public int IndexInScore => score.IndexOf(this);
        public ScoreDocumentCore HostScoreDocument => score;


        public InstrumentRibbon(ScoreDocumentCore score, Instrument instrument, ScoreDocumentStyleTemplate styleTemplate, InstrumentRibbonLayout layout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            Instrument = instrument;
            Layout = layout;
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
                Id = Guid,
                Instrument = Instrument,
                IndexInScore = IndexInScore,
                InstrumentMeasures = EnumerateMeasuresCore().Select(m => m.GetMemento()).ToArray(),
                Layout = Layout.GetMemento()
            };
        }
        public void ApplyMemento(InstrumentRibbonMemento memento)
        {
            Layout.ApplyMemento(memento.Layout);
            foreach (var measureMemento in memento.InstrumentMeasures)
            {
                var measure = GetMeasureCore(measureMemento.MeasureIndex);
                measure.ApplyMemento(measureMemento);
            }
        }
    }
}
