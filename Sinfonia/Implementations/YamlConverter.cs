using StudioLaValse.ScoreDocument.Layout.Templates;
using System.IO;
using YamlDotNet.Serialization;

namespace Sinfonia.Implementations
{
    internal class YamlConverter : IYamlConverter
    {
        private readonly SerializerBuilder serializerBuilder = new();
        private readonly DeserializerBuilder deserializerBuilder = new DeserializerBuilder();
        public YamlConverter()
        {
            
        }
        public ScoreDocumentStyleTemplate FromYaml(TextReader yaml)
        {
            return deserializerBuilder.Build().Deserialize<ScoreDocumentStyleTemplate>(yaml);
        }

        public string ToYaml(ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            return serializerBuilder.Build().Serialize(scoreDocumentStyleTemplate);
        }
    }
}
