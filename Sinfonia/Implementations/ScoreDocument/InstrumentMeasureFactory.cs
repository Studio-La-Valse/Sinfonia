using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class InstrumentMeasureFactory(IKeyGenerator<int> keyGenerator)
    {
        private readonly IKeyGenerator<int> keyGenerator = keyGenerator;

        public InstrumentMeasure Create(ScoreMeasure column, InstrumentRibbon row, ScoreDocumentStyleTemplate styleTemplate)
        {
            return new InstrumentMeasure(column, row, styleTemplate, keyGenerator, Guid.NewGuid());
        }
    }
}
