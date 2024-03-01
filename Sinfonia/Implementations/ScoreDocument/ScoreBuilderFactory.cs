using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class EmptyScoreBuilderFactory : IScoreBuilderFactory
    {
        private readonly IScoreDocumentStyleProvider scoreDocumentStyleProvider;

        public EmptyScoreBuilderFactory(IScoreDocumentStyleProvider scoreDocumentStyleProvider)
        {
            this.scoreDocumentStyleProvider = scoreDocumentStyleProvider;
        }

        public (IScoreBuilder builder, IScoreDocumentReader document, IScoreLayoutDictionary layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            var keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();
            var cellFactory = new InstrumentMeasureFactory(keyGenerator);
            var contentTable = new ScoreContentTable(cellFactory);
            var staffSystemGenerator = new StaffSystemGenerator(keyGenerator);
            var guid = Guid.NewGuid();

            var score = new ScoreDocumentCore(contentTable, keyGenerator, guid);
            var scoreEditor = score.ProxyEditor(commandManager, notifyEntityChanged);
            var scoreReader = score.Proxy(commandManager, notifyEntityChanged);

            var documentStyle = scoreDocumentStyleProvider.GetStyles().First();

            var defaultLayout = new ScoreLayoutDictionary(documentStyle, staffSystemGenerator);

            var builder = new ScoreBuilder(scoreEditor, defaultLayout, commandManager, notifyEntityChanged);

            return (builder, scoreReader, defaultLayout);
        }
    }
}
