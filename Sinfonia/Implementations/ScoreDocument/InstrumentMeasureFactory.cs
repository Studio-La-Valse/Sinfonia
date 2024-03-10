namespace Sinfonia.Implementations.ScoreDocument
{
    internal class InstrumentMeasureFactory(IKeyGenerator<int> keyGenerator)
    {
        private readonly IKeyGenerator<int> keyGenerator = keyGenerator;

        public InstrumentMeasure Create(ScoreMeasure column, InstrumentRibbon row)
        {
            return new InstrumentMeasure(column, row, keyGenerator, Guid.NewGuid());
        }
    }
}
