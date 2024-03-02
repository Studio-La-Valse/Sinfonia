namespace Sinfonia.Native.Scenes.PianoRoll
{
    public sealed class PianoRollScene : BaseExternalAddin, IExternalScene
    {
        private readonly IApplication application;

        public string Name => "Piano roll";
        public string Description => "Displays the score as a piano roll.";
        public Guid Guid => new Guid("804B8EB1-7D6A-4148-957B-588401754570");
        public double NoteHeight { get; set; } = 5;


        public PianoRollScene(IApplication application)
        {
            this.application = application;
        }


        public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument, IScoreLayoutProvider scoreLayoutDictionary)
        {
            var document = application.ActiveDocumentOrThrow();
            var selection = document.Selection;

            return new PianoRoll(scoreDocument, selection, scoreLayoutDictionary, () => NoteHeight);
        }

        public override void RegisterSettings(IAddinSettingsManager animationSettingsManager)
        {
            animationSettingsManager.Register(
                () => NoteHeight,
                (val) =>
                {
                    NoteHeight = val;
                    var document = application.ActiveDocumentOrThrow().DocumentUI;
                    document.RebuildScene();
                },
                "Note size");
        }
    }
}
