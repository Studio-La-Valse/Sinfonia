namespace Sinfonia.API
{
    public interface IExternalScene : IExternalAddin
    {
        BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument);
        void RegisterSettings(IAddinSettingsManager animationSettingsManager);
    }
}