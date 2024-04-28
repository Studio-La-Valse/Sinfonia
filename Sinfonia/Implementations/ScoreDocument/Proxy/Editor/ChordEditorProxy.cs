
using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Core;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class ChordEditorProxy : IChordEditor, IUniqueScoreElement
    {
        private readonly Chord source;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


        public InstrumentMeasure HostMeasure => source.HostMeasure;

        public bool Grace => source.Grace;

        public Position Position => source.Position;

        public RythmicDuration RythmicDuration => source.RythmicDuration;

        public Tuplet Tuplet => source.Tuplet;

        public Guid Guid => source.Guid;

        public int Id => source.Id;


        public ChordEditorProxy(Chord source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }



        public void Add(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Add(pitches));
            transaction.Enqueue(command);
        }

        public void Set(params Pitch[] pitches)
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Set(pitches));
            transaction.Enqueue(command);
        }

        public void Clear()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            MementoCommand<Chord, ChordMemento> command = new(source, s => s.Clear());
            transaction.Enqueue(command);
        }

        public IEnumerable<INoteEditor> ReadNotes()
        {
            return source.EnumerateNotesCore().Select(n => n.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadNotes();
        }

        public void RemoveLayout()
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new RestoreLayoutCommand<ChordLayout, ChordLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
            transaction.Enqueue(command);
        }

        public IChordLayout ReadLayout()
        {
            return source.Layout;
        }
    }
}
