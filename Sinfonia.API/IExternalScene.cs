using StudioLaValse.Drawable.ContentWrappers;
using StudioLaValse.Drawable.Interaction.Selection;
using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Layout;

namespace Sinfonia.API
{
    public interface IExternalScene : IExternalAddin
    {
        BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutProvider scoreLayoutDictionary, ISelection<IUniqueScoreElement> selection);
        void RegisterSettings(IAddinSettingsManager animationSettingsManager);
    }
}