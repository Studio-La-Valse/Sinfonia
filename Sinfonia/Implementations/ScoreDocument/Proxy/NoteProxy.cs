namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class NoteProxy : INoteEditor, INoteReader
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
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Note, NoteMemento>(source, s => s.Pitch = value);
                transaction.Enqueue(command);
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


        public AccidentalDisplay ForceAccidental
        {
            get => source.ForceAccidental;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Note, NoteMemento>(source, s => s.ForceAccidental = value);
                transaction.Enqueue(command);
            }
        }
        public int StaffIndex
        {
            get => source.StaffIndex;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Note, NoteMemento>(source, s => s.StaffIndex = value);
                transaction.Enqueue(command);
            }
        }
        public double XOffset
        {
            get => source.XOffset;
            set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Note, NoteMemento>(source, s => s.XOffset = value).ThenRerender(notifyEntityChanged, Host);
                transaction.Enqueue(command);
            }
        }

        public NoteProxy(Note source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
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

        public IChordReader ReadContext()
        {
            return source.ReadContext().Proxy(commandManager, notifyEntityChanged);
        }
    }
}
