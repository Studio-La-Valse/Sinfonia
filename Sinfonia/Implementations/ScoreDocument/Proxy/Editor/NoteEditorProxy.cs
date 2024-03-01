namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class NoteEditorProxy : INoteEditor
    {
        private readonly Note source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public IInstrumentMeasure Host => source.ReadContext().ReadContext().Proxy(commandManager, notifyEntityChanged);

        public Pitch Pitch
        {
            get
            {
                return source.Pitch;
            }
        }
        public bool Grace =>
            source.Grace;
        public Position Position =>
            source.Position;
        public RythmicDuration RythmicDuration =>
            source.RythmicDuration;
        public Tuplet Tuplet =>
            source.Tuplet;
        public int Id =>
            source.Id;
        public Guid Guid =>
            source.Guid;





        public NoteEditorProxy(Note source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }
    }
}
