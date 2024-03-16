//using StudioLaValse.Drawable.ContentWrappers;
//using StudioLaValse.ScoreDocument;
//using StudioLaValse.ScoreDocument.Core.Primitives;
//using StudioLaValse.ScoreDocument.Layout;

//namespace Sinfonia.API
//{
//    public interface IExternalScene : IExternalAddin
//    {
//        IExternalSceneContext CreateContext(IDocument document);
//    }

//    public interface IExternalSceneContext
//    {
//        IScoreDocumentLayout ScoreDocumentLayout { get; }
//        void RegisterSettings(IAddinSettingsManager animationSettingsManager);
//        BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument);
//    }
//}