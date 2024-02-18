namespace Sinfonia.Implementations.ScoreDocument.Proxy
{
    internal class ChordProxy : IChordEditor, IChordReader
    {
        private readonly Chord source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public bool Grace =>
            source.Grace;
        public Position Position =>
            source.Position;
        public RythmicDuration RythmicDuration =>
            source.RythmicDuration;
        public Tuplet Tuplet =>
            source.Tuplet;
        public int IndexInBlock =>
            source.IndexInBlock;



        public double XOffset
        {
            get => source.XOffset; set
            {
                var transaction = commandManager.ThrowIfNoTransactionOpen();
                var command = new MementoCommand<Chord, ChordMemento>(source, s => s.XOffset = value);
                transaction.Enqueue(command);
            }
        }

        public int Id => source.Id;

        public ChordProxy(Chord source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void Add(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Add(pitches));
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Set(pitches));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Chord, ChordMemento>(source, s => s.Clear());
            transaction.Enqueue(command);
        }



        public IEnumerable<INoteEditor> EditNotes()
        {
            return source.EnumerateNotesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public void SetBeamType(PowerOfTwo flag, BeamType beamType)
        {
            source.SetBeamType(flag, beamType);
        }

        public IInstrumentMeasureReader ReadContext()
        {
            return source.ReadContext().Proxy(commandManager, notifyEntityChanged);
        }

        public IEnumerable<INoteReader> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public BeamType? GetBeamType(PowerOfTwo flag)
        {
            return source.GetBeamType(flag);
        }

        public IEnumerable<(BeamType beam, PowerOfTwo duration)> GetBeamTypes()
        {
            return source.GetBeamTypes();
        }

        public IEnumerable<INote> EnumerateNotes()
        {
            return source.EnumerateNotesCore().Select(e => e.Proxy(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IUniqueScoreElement> EnumerateChildren()
        {
            return source.EnumerateChildren();
        }

        public bool Equals(IUniqueScoreElement? other)
        {
            return source.Equals(other);
        }
    }
}
