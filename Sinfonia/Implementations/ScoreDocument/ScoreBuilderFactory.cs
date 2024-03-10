using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class EmptyScoreBuilderFactory : IScoreBuilderFactory
    {
        private readonly ScoreDocumentStyleTemplate scoreDocumentStyle;

        public EmptyScoreBuilderFactory(ScoreDocumentStyleTemplate scoreDocumentStyle)
        {
            this.scoreDocumentStyle = scoreDocumentStyle;
        }

        public (IScoreBuilder builder, IScoreDocumentReader document, IScoreLayoutProvider layout) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            IKeyGenerator<int> keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();
            InstrumentMeasureFactory cellFactory = new(keyGenerator);
            ScoreContentTable contentTable = new(cellFactory);
            Guid guid = Guid.NewGuid();

            ScoreLayoutDictionary layoutDictionary = new(scoreDocumentStyle, commandManager, notifyEntityChanged);
            PageGenerator pageGenerator = new(keyGenerator, layoutDictionary);
            ScoreDocumentCore score = new(contentTable, pageGenerator, keyGenerator, guid);
            ScoreDocumentReaderProxy scoreReader = score.Proxy();
            ScoreDocumentEditorProxy scoreEditor = score.ProxyEditor(layoutDictionary, commandManager, notifyEntityChanged);

            ScoreBuilder builder = new(scoreEditor, commandManager, notifyEntityChanged);

            return (builder, scoreReader, layoutDictionary);
        }
    }
}
