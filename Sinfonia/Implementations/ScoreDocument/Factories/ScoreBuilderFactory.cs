using Sinfonia.Implementations.ScoreDocument.Proxy;

namespace Sinfonia.Implementations.ScoreDocument.Factories
{
    internal class EmptyScoreBuilderFactory : IScoreBuilderFactory
    {
        public EmptyScoreBuilderFactory()
        {

        }

        public (IScoreBuilder builder, IScoreDocumentReader document) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            var keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();
            var contentTable = new ScoreContentTable(keyGenerator);
            var guid = Guid.NewGuid();
            var score = new ScoreDocumentCore(contentTable, keyGenerator, guid).Proxy(commandManager, notifyEntityChanged);

            var builder = new ScoreBuilder(score, commandManager, notifyEntityChanged)
                .Edit(builder =>
                {
                    builder.AddInstrumentRibbon(Instrument.Piano);
                    builder.AddInstrumentRibbon(Instrument.Violin);
                })
                .Edit(builder =>
                {
                    for (int i = 0; i < 16; i++)
                    {
                        builder.AppendScoreMeasure();
                    }
                })
                .Edit(builder =>
                {
                    for (int i = 0; i < 16; i++)
                    {
                        if (i % 4 == 0)
                        {
                            var measure = builder.EditScoreMeasure(i);
                            measure.IsNewSystem = true;
                        }
                    }
                })
                .Build();

            return (builder, score);
        }
    }
}
