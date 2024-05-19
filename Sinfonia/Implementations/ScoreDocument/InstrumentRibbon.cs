using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;
using Sinfonia.Implementations.ScoreDocument.Converters;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentRibbon : ScoreElement, IMementoElement<InstrumentRibbonModel>
    {
        private readonly ScoreDocumentCore score;

        public Instrument Instrument { get; }
        public InstrumentRibbonLayout Layout { get; }
        public SecondaryInstrumentRibbonLayout SecondaryLayout { get; }

        public int IndexInScore => score.IndexOf(this);
        public ScoreDocumentCore HostScoreDocument => score;


        public InstrumentRibbon(ScoreDocumentCore score,
                                Instrument instrument,
                                InstrumentRibbonLayout layout,
                                SecondaryInstrumentRibbonLayout secondaryLayout,
                                IKeyGenerator<int> keyGenerator,
                                Guid guid) : base(keyGenerator, guid)
        {
            this.score = score;

            Instrument = instrument;
            Layout = layout;
            SecondaryLayout = secondaryLayout;
        }


        public InstrumentMeasure GetMeasureCore(int index)
        {
            return score.GetMeasureCore(index, IndexInScore);
        }


        public IEnumerable<InstrumentMeasure> EnumerateMeasuresCore()
        {
            return score.EnumerateScoreMeasuresCore(this);
        }




        public InstrumentRibbonModel GetMemento()
        {
            return new InstrumentRibbonModel
            {
                Id = Guid,
                Instrument = Instrument.Convert(),
                IndexInScore = IndexInScore,
                Layout = SecondaryLayout.GetMemento(),
                AbbreviatedName = Layout.abbreviatedName.Field,
                Collapsed = Layout.collapsed.Field,
                DisplayName = Layout.displayName.Field,
                NumberOfStaves = Layout.numberOfStaves.Field
            };
        }
        public void ApplyMemento(InstrumentRibbonModel memento)
        {
            Layout.ApplyMemento(memento);   
            SecondaryLayout.ApplyMemento(memento.Layout);
        }
    }
}
