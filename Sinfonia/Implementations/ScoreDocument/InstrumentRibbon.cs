namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonMemento>
    {
        private readonly ScoreDocumentCore score;

        public Instrument Instrument { get; }


        public int IndexInScore => score.contentTable.IndexOf(this);
        public ScoreDocumentCore HostScoreDocument => score;


        public InstrumentRibbon(ScoreDocumentCore score, Instrument instrument, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;
            Instrument = instrument;
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
            foreach (InstrumentMeasureMemento measureMemento in memento.Measures)
            {
                InstrumentMeasure measure = GetMeasureCore(measureMemento.MeasureIndex);
                measure.ApplyMemento(measureMemento);
            }
        }
    }
}
