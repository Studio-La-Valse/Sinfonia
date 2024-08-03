using StudioLaValse.ScoreDocument.Models;

namespace Sinfonia.Interfaces
{
    public interface IDocumentViewModelFactory
    {
        DocumentViewModel Create(ScoreDocumentModel scoreDocument, ScoreDocumentMetaDataModel metaData);
    }
}
