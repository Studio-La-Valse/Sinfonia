//using StudioLaValse.Drawable.Interaction.Selection;
//using StudioLaValse.ScoreDocument.Drawable.PageView.Layout;
//using StudioLaValse.ScoreDocument.Drawable.PageView.Layout.Templates;
//using StudioLaValse.ScoreDocument.Drawable.PageView.Scenes;
//using StudioLaValse.ScoreDocument.Layout;

//namespace Sinfonia.Native.Scenes.MultiPageView
//{
//    public class MultiPageScene : BaseExternalAddin, IExternalScene
//    {
//        public string Name => "Multi page view";
//        public string Description => "Displays all the pages from the score document.";
//        public Guid Guid => new("AC454ABB-F73F-4A67-BA2F-8B1C2DD3CC61");



//        public MultiPageScene()
//        {

//        }


        

//        public IExternalSceneContext CreateContext(IDocument document)
//        {
//            return new MultiPageSceneContext(document);
//        }
//    }

//    public class MultiPageSceneContext : IExternalSceneContext
//    {
//        private readonly ISelection<IUniqueScoreElement> selection;
//        private readonly ScoreDocumentStyleTemplate styleTemplate;
//        private readonly ScoreLayoutDictionary layoutDictionary;
//        private readonly PageGenerator pageGenerator;

//        public MultiPageSceneContext(IDocument document)
//        {
//            this.selection = document.Selection;
//            this.styleTemplate = new ScoreDocumentStyleTemplate();
//            this.layoutDictionary = new ScoreLayoutDictionary(styleTemplate);
//            this.pageGenerator = new PageGenerator(document.KeyGenerator, layoutDictionary, document.ScoreReader);
//        }

//        public IScoreDocumentLayout ScoreDocumentLayout => layoutDictionary;

//        public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument)
//        {
            
//            return new VisualScoreDocumentScene(scene, scoreDocument);
//        }

//        public void RegisterSettings(IAddinSettingsManager animationSettingsManager)
//        {
//            animationSettingsManager.Register(() => styleTemplate.NoteStyleTemplate.Scale, val => styleTemplate.NoteStyleTemplate.Scale = val, "Scale");

//            animationSettingsManager.RegisterLayoutProperty<NoteLayout, int>("staffindex", "Staff Index");
//        }
//    }
//}
