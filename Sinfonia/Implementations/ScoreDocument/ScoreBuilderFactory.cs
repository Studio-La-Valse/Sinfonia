using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class EmptyScoreBuilderFactory : IScoreBuilderFactory
    {
        private readonly IScoreDocumentStyleProvider scoreDocumentStyleProvider;

        public EmptyScoreBuilderFactory(IScoreDocumentStyleProvider scoreDocumentStyleProvider)
        {
            this.scoreDocumentStyleProvider = scoreDocumentStyleProvider;
        }

        public (IScoreBuilder builder, IScoreDocumentReader document, IScoreLayoutProvider layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            var keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();
            var cellFactory = new InstrumentMeasureFactory(keyGenerator);
            var contentTable = new ScoreContentTable(cellFactory);
            var guid = Guid.NewGuid();

            var systemGenerator = new StaffSystemGenerator(keyGenerator);
            var documentStyle = scoreDocumentStyleProvider.GetStyles().First();

            var layoutDictionary = new ScoreLayoutDictionary(documentStyle, commandManager);
            var score = new ScoreDocumentCore(layoutDictionary, contentTable, systemGenerator, keyGenerator, guid);
            var scoreReader = score.Proxy();
            var scoreEditor = score.ProxyEditor(layoutDictionary, commandManager, notifyEntityChanged);

            var builder = new ScoreBuilder(scoreEditor, layoutDictionary, commandManager, notifyEntityChanged);

            return (builder, scoreReader, layoutDictionary);
        }
    }
}
