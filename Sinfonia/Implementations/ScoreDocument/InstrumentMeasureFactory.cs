using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentMeasureFactory(IKeyGenerator<int> keyGenerator)
    {
        private readonly IKeyGenerator<int> keyGenerator = keyGenerator;

        public InstrumentMeasure Create(ScoreMeasure column, InstrumentRibbon row, ScoreDocumentStyleTemplate styleTemplate)
        {
            var layout = new InstrumentMeasureLayout(row.Instrument, column);
            var secondaryLayout = new SecondaryInstrumentMeasureLayout(layout, Guid.NewGuid());
            return new InstrumentMeasure(column, row, styleTemplate, layout, secondaryLayout, keyGenerator, Guid.NewGuid());
        }
    }
}
