namespace Sinfonia.Native.Scenes.MultiPageView
{
    public class MultiPageScene : BaseExternalAddin, IExternalScene
    {
        private readonly IApplication application;


        public string Name => "Multi page view";
        public string Description => "Displays all the pages from the score document.";
        public Guid Guid => new Guid("AC454ABB-F73F-4A67-BA2F-8B1C2DD3CC61");


        public MultiPageScene(IApplication application)
        {
            this.application = application;
        }


        public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument)
        {
            var document = application.ActiveDocumentOrThrow();
            var selection = document.Selection;

            var noteFactory = new VisualNoteFactory(selection);
            var restFactory = new VisualRestFactory(selection);
            var noteGroupFactory = new VisualNoteGroupFactory(noteFactory, restFactory);
            var staffMeasusureFactory = new VisualStaffMeasureFactory(selection, noteGroupFactory);
            var systemMeasureFactory = new VisualSystemMeasureFactory(selection, staffMeasusureFactory);
            var staffSystemFactory = new VisualStaffSystemFactory(systemMeasureFactory, selection);
            var scene = new PageViewSceneFactory(staffSystemFactory, PageSize.A4, 20, 30, ColorARGB.Black, ColorARGB.White);
            return new VisualScoreDocumentScene(scene, scoreDocument);
        }
    }
}
