using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class EmptyScoreBuilderFactory : IScoreBuilderFactory
    {
        public EmptyScoreBuilderFactory()
        {
        }

        public (IScoreBuilder builder, IScoreDocumentReader document, IScoreDocumentLayout layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged, ScoreDocumentStyleTemplate scoreStyleTemplate)
        {
            var keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();

            InstrumentMeasureFactory cellFactory = new(keyGenerator);
            ScoreContentTable contentTable = new(cellFactory);
            var guid = Guid.NewGuid();

            ScoreLayoutDictionary layout = new(scoreStyleTemplate, commandManager, notifyEntityChanged);
            PageGenerator pageGenerator = new(keyGenerator, layout);

            ScoreDocumentCore score = new(contentTable, pageGenerator, keyGenerator, guid);

            var scoreReader = score.Proxy();
            var scoreEditor = score.ProxyEditor(layout, commandManager, notifyEntityChanged);

            ScoreBuilder builder = new(scoreEditor, commandManager, notifyEntityChanged);

            return (builder, scoreReader, layout);
        }
    }
}