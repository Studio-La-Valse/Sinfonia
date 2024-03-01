namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentMeasureFactory
    {
        private readonly IKeyGenerator<int> keyGenerator;

        public InstrumentMeasureFactory(IKeyGenerator<int> keyGenerator)
        {
            this.keyGenerator = keyGenerator;
        }

        public InstrumentMeasure Create(ScoreMeasure column, InstrumentRibbon row)
        {
            return new InstrumentMeasure(column, row, keyGenerator, Guid.NewGuid());
        }
    }
}
