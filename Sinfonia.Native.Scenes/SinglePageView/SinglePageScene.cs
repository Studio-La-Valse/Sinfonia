using Microsoft.Extensions.DependencyInjection;
using StudioLaValse.DependencyInjection;

namespace Sinfonia.Native.Scenes.SinglePageView
{
    public class SinglePageScene : BaseExternalAddin, IExternalScene
    {
        private readonly IApplication application;

        public Guid Guid { get; } = new Guid("189FB397-96C8-453B-A0A4-BD73A8F83926");
        public string Name => "Single page view";
        public string Description => "Displays a single page from the score document.";
        public int Page { get; set; }


        public SinglePageScene(IApplication application)
        {
            this.application = application;
        }

        public override void RegisterSettings(IAddinSettingsManager animationSettingsManager)
        {
            animationSettingsManager.Register(
                () => Page,
                (val) =>
                {
                    Page = val;
                    application.ActiveDocumentOrThrow().DocumentUI.RebuildScene();
                },
                "The page to display");
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
            return new SinglePageDocumentScene(staffSystemFactory, scoreDocument, () => Page);
        }
    }
}
