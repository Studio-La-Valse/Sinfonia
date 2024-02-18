using Sinfonia.Implementations.ScoreDocument.Layout.Elements;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class CellFactory : ICellFactory<InstrumentMeasure, ScoreMeasure, InstrumentRibbon>
    {
        private readonly IKeyGenerator<int> keyGenerator;

        public CellFactory(IKeyGenerator<int> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }

        public InstrumentMeasure Create(ScoreMeasure column, InstrumentRibbon row)
        {
            var layout = new InstrumentMeasureLayout();
            return new InstrumentMeasure(column, row, layout, keyGenerator, Guid.NewGuid());
        }
    }
}
