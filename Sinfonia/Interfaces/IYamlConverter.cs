using StudioLaValse.ScoreDocument.Layout.Templates;
using System.IO;

namespace Sinfonia.Interfaces
{
    public interface IYamlConverter
    {
        string ToYaml(ScoreDocumentStyleTemplate scoreDocumentStyleTemplate);
        ScoreDocumentStyleTemplate FromYaml(TextReader yaml);
    }
}
