using StudioLaValse.ScoreDocument.Layout.Templates;

namespace Sinfonia.Interfaces
{
    public interface IScoreStyleTemplateSaveService
    {
        void Save(ScoreDocumentStyleTemplate scoreDocumentStyleTemplate);
        ScoreDocumentStyleTemplate Open();
    }
}
