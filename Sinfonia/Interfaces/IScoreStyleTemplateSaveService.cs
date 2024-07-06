using StudioLaValse.ScoreDocument.StyleTemplates;

namespace Sinfonia.Interfaces
{
    public interface IScoreStyleTemplateSaveService
    {
        void Save(ScoreDocumentStyleTemplate scoreDocumentStyleTemplate);
        ScoreDocumentStyleTemplate? Open();
    }
}
