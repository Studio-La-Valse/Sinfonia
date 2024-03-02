namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonMemento>
    {
        private readonly ScoreDocumentCore score;
        private readonly Instrument instrument;




        public Instrument Instrument =>
            instrument;
        public int IndexInScore =>
            score.contentTable.IndexOf(this);



        public InstrumentRibbon(ScoreDocumentCore score, Instrument instrument, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;
            this.instrument = instrument;
        }


        public InstrumentMeasure GetMeasureCore(int index)
        {
            return score.contentTable.GetInstrumentMeasure(index, IndexInScore);
        }


        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            return score.contentTable.GetInstrumentMeasuresInInstrumentRibbon(IndexInScore);
        }




        public InstrumentRibbonMemento GetMemento()
        {
            return new InstrumentRibbonMemento
            {
                Measures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                Instrument = Instrument,
                Guid = Guid,
            };
        }
        public void ApplyMemento(InstrumentRibbonMemento memento)
        {
            foreach (var measureMemento in memento.Measures)
            {
                var measure = GetMeasureCore(measureMemento.MeasureIndex);
                measure.ApplyMemento(measureMemento);
            }
        }
    }
}
