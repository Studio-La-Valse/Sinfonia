
using Sinfonia.Implementations.Commands;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ChordEditorProxy : IChordEditor, IUniqueScoreElement
    {
        private readonly Chord source;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public InstrumentMeasure HostMeasure => source.HostMeasure;

        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public ChordEditorProxy(Chord source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void Add(params Pitch[] pitches)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Add(pitches));
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Set(pitches));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public IEnumerable<INoteEditor> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(n => n.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public ChordLayout ReadLayout()
        {
            return scoreLayoutDictionary.ChordLayout(this);
        }

        public void Apply(ChordLayout layout)
        {
            scoreLayoutDictionary.Apply(this, layout);
        }

        public void RemoveLayout()
        {
            scoreLayoutDictionary.Restore(this);
        }
    }
}
