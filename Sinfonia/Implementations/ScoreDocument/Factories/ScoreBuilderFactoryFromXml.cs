using Sinfonia.Implementations.ScoreDocument.Factories.MusicXML;
using Sinfonia.Implementations.ScoreDocument.Proxy;
using System.Xml.Linq;

namespace Sinfonia.Implementations.ScoreDocument.Factories
{
    internal class ScoreBuilderFactoryFromXml : IScoreBuilderFactory
    {
        private readonly XDocument document;


        public ScoreBuilderFactoryFromXml(XDocument document)
        {
            this.document = document;
        }

        public (IScoreBuilder builder, IScoreDocumentReader document) Create(ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            var keyGenerator = new IncrementalIntGeneratorFactory().CreateKeyGenerator();
            var contentTable = new ScoreContentTable(keyGenerator);
            var guid = Guid.NewGuid();
            var score = new ScoreDocumentCore(contentTable, keyGenerator, guid);

            var chainConverter = new BlockChainXmlConverter();
            var measureConverter = new ScorePartMeasureXmlConverter(chainConverter);
            var partConverter = new ScorePartXmlConverter(measureConverter);
            var converter = new ScoreDocumentXmlConverter(partConverter);

            converter.Create(document, score);
            var proxy = score.Proxy(commandManager, notifyEntityChanged);

            var builder = new ScoreBuilder(proxy, commandManager, notifyEntityChanged);
            return (builder, proxy);
        }
    }
}
