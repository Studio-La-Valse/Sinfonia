using Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Reader
{
    internal class ChordReaderProxy : IChordReader
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





        public int Id => source.Id;

        public Guid Guid => source.Guid;

        public ChordReaderProxy(Chord source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
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




        public IEnumerable<INoteReader> ReadNotes()
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

        public IEnumerable<(BeamType beam, PowerOfTwo duration)> ReadBeamTypes()
        {
            return source.GetBeamTypes();
        }

        public BeamType? ReadBeamType(PowerOfTwo i)
        {
            return source.GetBeamType(i);
        }
    }
}
