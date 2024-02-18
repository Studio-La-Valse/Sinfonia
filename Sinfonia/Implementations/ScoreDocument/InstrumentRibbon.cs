using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonMemento>
    {
        private readonly ScoreDocumentCore score;
        private readonly Instrument instrument;
        private IInstrumentRibbonLayout layout;


        public Guid Guid { get; }



        public Instrument Instrument =>
            instrument;
        public int IndexInScore =>
            score.contentTable.IndexOf(this);

        public string AbbreviatedName { get => ReadLayout().AbbreviatedName; set => ReadLayout().AbbreviatedName = value; }
        public bool Collapsed { get => ReadLayout().Collapsed; set => ReadLayout().Collapsed = value; }
        public string DisplayName { get => ReadLayout().DisplayName; set => ReadLayout().DisplayName = value; }
        public int NumberOfStaves { get => ReadLayout().NumberOfStaves; set => ReadLayout().NumberOfStaves = value; }

        public InstrumentRibbon(ScoreDocumentCore score, Instrument instrument, IInstrumentRibbonLayout layout, IKeyGenerator<int> keyGenerator, Guid guid) : base(keyGenerator)
        {
            this.score = score;
            this.instrument = instrument;
            this.layout = layout;

            Guid = guid;
        }


        public InstrumentMeasure GetMeasureCore(int index)
        {
            return score.contentTable.GetCell(index, IndexInScore);
        }


        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            return score.contentTable.GetCellsRow(IndexInScore);
        }




        public void ApplyLayout(IInstrumentRibbonLayout layout)
        {
            this.layout = layout;
        }
        public IInstrumentRibbonLayout ReadLayout()
        {
            return layout;
        }



        public InstrumentRibbonMemento GetMemento()
        {
            return new InstrumentRibbonMemento
            {
                Layout = ReadLayout().Copy(),
                Measures = EnumerateMeasuresCore().Select(e => e.GetMemento()).ToList(),
                Instrument = Instrument,
                Guid = Guid,
            };
        }
        public void ApplyMemento(InstrumentRibbonMemento memento)
        {
            ApplyLayout(memento.Layout);

            foreach (var measureMemento in memento.Measures)
            {
                var measure = GetMeasureCore(measureMemento.MeasureIndex);
                measure.ApplyMemento(measureMemento);
            }
        }

        public override IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return EnumerateMeasuresCore();
        }
    }
}
