namespace Sinfonia.API
{
    public interface IExternalScene : IExternalAddin
    {
        BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutProvider scoreLayoutDictionary);
        void RegisterSettings(IAddinSettingsManager animationSettingsManager);
    }
}