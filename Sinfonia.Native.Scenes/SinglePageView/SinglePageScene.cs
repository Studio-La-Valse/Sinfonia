//namespace Sinfonia.Native.Scenes.SinglePageView
//{
//    public class SinglePageScene : BaseExternalAddin, IExternalScene
//    {
//        private readonly IApplication application;

//        public Guid Guid { get; } = new Guid("189FB397-96C8-453B-A0A4-BD73A8F83926");
//        public string Name => "Single page view";
//        public string Description => "Displays a single page from the score document.";
//        public int Page { get; set; }


//        public SinglePageScene(IApplication application)
//        {
//            this.application = application;
//        }

//        public override void RegisterSettings(IAddinSettingsManager animationSettingsManager)
//        {
//            animationSettingsManager.Register(
//                () => Page,
//                (val) =>
//                {
//                    Page = val;
//                    application.ActiveDocumentOrThrow().DocumentUI.RebuildScene();
//                },
//                "The page to display");
//        }

//        public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutProvider layout, ISelection<IUniqueScoreElement> selection)
//        {
//            VisualNoteFactory noteFactory = new(selection, layout);
//            VisualRestFactory restFactory = new(selection);
//            VisualNoteGroupFactory noteGroupFactory = new(noteFactory, restFactory, layout);
//            VisualStaffMeasureFactory staffMeasusureFactory = new(selection, noteGroupFactory, layout);
//            VisualSystemMeasureFactory systemMeasureFactory = new(selection, staffMeasusureFactory, layout);
//            VisualStaffSystemFactory staffSystemFactory = new(systemMeasureFactory, selection, layout);
//            return new SinglePageDocumentScene(staffSystemFactory, scoreDocument, layout, () => Page);
//        }
//    }
//}
