namespace Sinfonia.Native.Scenes.MultiPageView
{
    public class MultiPageScene : BaseExternalAddin, IExternalScene
    {
        private readonly IApplication application;


        public string Name => "Multi page view";
        public string Description => "Displays all the pages from the score document.";
        public Guid Guid => new("AC454ABB-F73F-4A67-BA2F-8B1C2DD3CC61");


        public MultiPageScene(IApplication application)
        {
            this.application = application;
        }


        public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutProvider layoutDictionary, ISelection<IUniqueScoreElement> selection)
        {
            VisualNoteFactory noteFactory = new(selection, layoutDictionary);
            VisualRestFactory restFactory = new(selection);
            VisualNoteGroupFactory noteGroupFactory = new(noteFactory, restFactory, layoutDictionary);
            VisualStaffMeasureFactory staffMeasusureFactory = new(selection, noteGroupFactory, layoutDictionary);
            VisualSystemMeasureFactory systemMeasureFactory = new(selection, staffMeasusureFactory, layoutDictionary);
            VisualStaffSystemFactory staffSystemFactory = new(systemMeasureFactory, selection, layoutDictionary);
            PageViewSceneFactory scene = new(staffSystemFactory, 20, 30, ColorARGB.Black, ColorARGB.White, layoutDictionary);
            return new VisualScoreDocumentScene(scene, scoreDocument);
        }
    }
}
