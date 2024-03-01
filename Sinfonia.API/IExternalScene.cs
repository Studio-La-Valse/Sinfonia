using IScoreLayoutDictionary = StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary;

namespace Sinfonia.API
{
    public interface IExternalScene : IExternalAddin
    {
        BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutDictionary scoreLayoutDictionary);
        void RegisterSettings(IAddinSettingsManager animationSettingsManager);
    }
}